using NAudio.Midi;
using Newtonsoft.Json.Linq;
using System.Threading.Channels;

namespace LBPMusicReader
{
    internal class LBPReader
    {
        static readonly float INGAME_NOTE_WIDTH = 105f;
        static readonly float INGAME_NOTE_HEIGHT = 105f;
		static readonly int INGAME_PAGE_SIZE = 32;
		static int sequencersExported = 0;

        static bool isSequencer(JObject obj)
        {
            return obj["PSequencer"] != null && obj["PMicrochip"] != null;
		}

		static float getSequencerTempo(JObject obj)
		{
			if (!isSequencer(obj))
				throw new Exception("Object is not a sequencer.");
			var sequencer = obj["PSequencer"];
			return (float)sequencer["tempo"];
		}

		static void ExportNotes(LbpNote[] notes,string filename,int tempo)
		{
			// export
			var sortedNotes = notes.OrderBy(n => n.globalStartTime).ToList();
			MidiEventCollection events = new MidiEventCollection(1, 70);
			// set bpm
			var tempoEvent = new TempoEvent(60000000 / tempo, 0);
			events.AddEvent(tempoEvent, 0);
			foreach (var note in sortedNotes)
			{
				events.AddEvent(new NoteOnEvent(note.globalStartTime, 1, note.NoteId, 100, 0), note.channelId);
				events.AddEvent(new NoteEvent(note.globalStartTime + note.length, 1, MidiCommandCode.NoteOff, note.NoteId, 0), note.channelId);
			}
			MidiFile.Export(filename, events);
		}

		static LbpNote[] decodeInstrumentNotes(JToken instrument,int channelId,float componentX)
		{
			List<LbpNote> tnotes = new List<LbpNote>();
			var mult = 17.5f; // TODO: find out what this multiplier actually means
			var notes = instrument["notes"] as JArray;
			LbpNote lastNote = null;
			foreach (var note in notes)
			{
				var end = (bool)note["end"];
				if (!end)
				{
					lastNote = new LbpNote()
					{
						length = 1,
						NoteId = (int)note["y"],
						startTime = (int)note["x"],
						globalStartTime = (int)((((componentX / INGAME_NOTE_WIDTH) * INGAME_PAGE_SIZE) + (int)note["x"]) * mult),
						channelId = channelId
					};
				}
				else
				{
					if (lastNote == null)
						continue;//throw new Exception("End note found without a starting note.");
					var endGlobalStartTime = (int)((((componentX / INGAME_NOTE_WIDTH) * INGAME_PAGE_SIZE) + (int)note["x"]) * mult);
					lastNote.length = endGlobalStartTime - lastNote.globalStartTime;
					tnotes.Add(lastNote);
					Console.WriteLine($"NoteId: {lastNote.NoteId}, StartTime: {lastNote.startTime} ({lastNote.globalStartTime}), Length: {lastNote.length}");
					lastNote = null;
				}
			}
			return tnotes.ToArray();
		}

        static LbpNote[] decodeSequencer(JObject obj)
        {
			List<int> chs = new List<int>();
			List<LbpNote> tnotes = new List<LbpNote>();
			var microchip = obj["PMicrochip"];
			var microchipWidth = (float)microchip["circuitBoardSizeX"];
			var microchipHeight = (float)microchip["circuitBoardSizeY"];
			var circuitBoardComponents = microchip["components"];
			List<LbpInstrumentComponent> instruments = new List<LbpInstrumentComponent>();
			if (circuitBoardComponents != null && circuitBoardComponents.Count() > 0)
			{
				// normal LBP3 sequencer
				foreach (var component in circuitBoardComponents)
				{
					float componentX = (float)component["x"];
					float componentY = (float)component["y"];
					componentX -= INGAME_NOTE_WIDTH / 2f;
					componentY += microchipHeight + INGAME_NOTE_HEIGHT / 2f;

					if (component["thing"] != null && component["thing"] is JObject && component["thing"]["PInstrument"] != null)
					{
						var instrument = component["thing"]["PInstrument"];
						LbpInstrumentComponent comp = new LbpInstrumentComponent()
						{
							componentX = componentX,
							componentY = componentY,
							rawData = instrument
						};
						instruments.Add(comp);
					}
				}
			}
			else if (circuitBoardComponents == null || circuitBoardComponents.Count() == 0)
			{
				// TODO: this might be a LBP2 sequencer?? components stored differently
				/*var cbThing = microchip["circuitBoardThing"];
				if (cbThing == null)
					throw new Exception("No circuit board components found in sequencer.");
				var UID = (int)cbThing["UID"];
				var whereToSearch = microchip.Parent.Parent.Parent;
				foreach (JToken thing in whereToSearch.Where(t => t is JObject))
				{
					var thingParent = thing["parent"] != null ? (thing["parent"]?.Type != JTokenType.Integer ? -1 : (int)thing["parent"]) : -1;
					if (thingParent == UID && thing["PInstrument"] != null && thing["PPos"] != null && thing["PPos"]["worldPosition"]["translation"] != null)
					{
						var instrument = thing["PInstrument"];
						var pos = thing["PPos"];
						Console.WriteLine($"{pos["worldPosition"]["translation"]}");
						LbpInstrumentComponent comp = new LbpInstrumentComponent()
						{
							componentX = 0,
							componentY = 0,
							rawData = instrument
						};
						instruments.Add(comp);
					}
				}*/
				Console.WriteLine("Support for this sequencer type not implemented yet.");
			}
			else
			{
				throw new Exception("No circuit board components found in sequencer.");
			}
			Console.WriteLine($"Found {instruments.Count} instruments associated with this sequencer.");

			foreach (var instrument in instruments)
			{
				if (!chs.Contains((int)instrument.componentY))
					chs.Add((int)instrument.componentY);
				int channelId = chs.IndexOf((int)instrument.componentY) + 1;
				var instrumentNotes = decodeInstrumentNotes(instrument.rawData, channelId, instrument.componentX);
				tnotes.AddRange(instrumentNotes);
			}
			return tnotes.ToArray();
		}

		public static void ProcessThing(JObject thing)
		{
			if (thing is not JObject obj) return;

			if (isSequencer(obj))
			{
				Console.WriteLine($"Found sequencer!");
				Console.WriteLine($"---");
				sequencersExported++;
				var name = $"sequencer_" + sequencersExported;
				var tnotes = decodeSequencer(obj).ToList();
				if (tnotes.Count == 0)
				{
					Console.WriteLine($"No notes found, skipping export.");
					return;
				}
				Console.WriteLine($"---");

				ExportNotes(tnotes.ToArray(), $"{name}.mid", (int)getSequencerTempo(obj));
			}
			else
			{
				// recursively search child objects/arrays for sequencers
				foreach (var property in obj.Properties())
				{
					var value = property.Value;
					if (value is JObject childObj)
					{
						ProcessThing(childObj);
					}
					else if (value is JArray arr)
					{
						foreach (var item in arr)
						{
							if (item is JObject itemObj)
							{
								ProcessThing(itemObj);
							}
						}
					}
				}

			}
		}

		static void Main(string[] args)
        {
            if (args.Length == 0)
			{
				Console.WriteLine("Please provide the path to a JSON file as an argument.");
				return;
			}
			var jsonLoc = args[0];
			var json = System.IO.File.ReadAllText(jsonLoc);

			var musicData = Newtonsoft.Json.JsonConvert.DeserializeObject<JToken>(json,new Newtonsoft.Json.JsonSerializerSettings()
			{
				 MaxDepth= 999 // Lazy workaround for deep JSON files
			});
			var dataType = musicData["type"]?.ToString();
			JToken things=null;
			if (dataType == "PLAN")
				things = musicData["resource"]["things"]; // this is for PLAN
			else if (dataType == "LEVEL")
				things = musicData["resource"]["worldThing"]["PWorld"]["things"];
			else
				Console.WriteLine($"Unknown data type {dataType}");

            foreach (var thing in things)
            {
                ProcessThing(thing as JObject);
			}
		}
	}
}
