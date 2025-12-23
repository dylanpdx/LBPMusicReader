using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBPMusicReader
{
	public enum MidiInstrument
	{
		// Piano
		AcousticGrandPiano = 0x00,
		BrightAcousticPiano = 0x01,
		ElectricGrandPiano = 0x02,
		HonkyTonkPiano = 0x03,
		ElectricPiano1 = 0x04,
		ElectricPiano2 = 0x05,
		Harpsicord = 0x06,
		Clavinet = 0x07,

		// Chromatic percussion
		Celesta = 0x08,
		Glockenspiel = 0x09,
		MusicBox = 0x0A,
		Vibraphone = 0x0B,
		Marimba = 0x0C,
		Xylophone = 0x0D,
		TubularBell = 0x0E,
		Dulcimer = 0x0F,

		// Organ
		HammondDrawbarOrgan = 0x10,
		PercussiveOrgan = 0x11,
		RockOrgan = 0x12,
		ChurchOrgan = 0x13,
		ReedOrgan = 0x14,
		Accordion = 0x15,
		Harmonica = 0x16,
		TangoAccordion = 0x17,

		// Guitar
		NylonStringAcousticGuitar = 0x18,
		SteelStringAcousticGuitar = 0x19,
		JazzElectricGuitar = 0x1A,
		CleanElectricGuitar = 0x1B,
		MutedElectricGuitar = 0x1C,
		OverdrivenGuitar = 0x1D,
		DistortionGuitar = 0x1E,
		GuitarHarmonics = 0x1F,

		// Bass
		AcousticBass = 0x20,
		FingeredElectricBass = 0x21,
		PickedElectricBass = 0x22,
		FretlessBass = 0x23,
		SlapBass1 = 0x24,
		SlapBass2 = 0x25,
		SynthBass1 = 0x26,
		SynthBass2 = 0x27,

		// Strings
		Violin = 0x28,
		Viola = 0x29,
		Cello = 0x2A,
		Contrabass = 0x2B,
		TremoloStrings = 0x2C,
		PizzicatoStrings = 0x2D,
		OrchestralStringsHarp = 0x2E,
		Timpani = 0x2F,

		// Ensemble
		StringEnsemble1 = 0x30,
		StringEnsemble2SlowStrings = 0x31,
		SynthStrings1 = 0x32,
		SynthStrings2 = 0x33,
		ChoirAahs = 0x34,
		VoiceOohs = 0x35,
		SynthChoirVoice = 0x36,
		OrchestraHit = 0x37,

		// Brass
		Trumpet = 0x38,
		Trombone = 0x39,
		Tuba = 0x3A,
		MutedTrumpet = 0x3B,
		FrenchHorn = 0x3C,
		BrassEnsemble = 0x3D,
		SynthBrass1 = 0x3E,
		SynthBrass2 = 0x3F,

		// Reed
		SopranoSax = 0x40,
		AltoSax = 0x41,
		TenorSax = 0x42,
		BaritoneSax = 0x43,
		Oboe = 0x44,
		EnglishHorn = 0x45,
		Bassoon = 0x46,
		Clarinet = 0x47,

		// Pipe
		Piccolo = 0x48,
		Flute = 0x49,
		Recorder = 0x4A,
		PanFlute = 0x4B,
		BottleBlowBlownBottle = 0x4C,
		Shakuhachi = 0x4D,
		Whistle = 0x4E,
		Ocarina = 0x4F,

		// Synth lead
		SynthSquareWave = 0x50,
		SynthSawWave = 0x51,
		SynthCalliope = 0x52,
		SynthChiff = 0x53,
		SynthCharang = 0x54,
		SynthVoice = 0x55,
		SynthFifthsSaw = 0x56,
		SynthBrassAndLead = 0x57,

		// Synth pad
		FantasiaNewAge = 0x58,
		WarmPad = 0x59,
		Polysynth = 0x5A,
		SpaceVoxChoir = 0x5B,
		BowedGlass = 0x5C,
		MetalPad = 0x5D,
		HaloPad = 0x5E,
		SweepPad = 0x5F,

		// Synth effects
		IceRain = 0x60,
		Soundtrack = 0x61,
		Crystal = 0x62,
		Atmosphere = 0x63,
		Brightness = 0x64,
		Goblins = 0x65,
		EchoDropsEchoes = 0x66,
		SciFi = 0x67,

		// Ethnic
		Sitar = 0x68,
		Banjo = 0x69,
		Shamisen = 0x6A,
		Koto = 0x6B,
		Kalimba = 0x6C,
		BagPipe = 0x6D,
		Fiddle = 0x6E,
		Shanai = 0x6F,

		// Percussive
		TinkleBell = 0x70,
		Agogo = 0x71,
		SteelDrums = 0x72,
		Woodblock = 0x73,
		TaikoDrum = 0x74,
		MelodicTom = 0x75,
		SynthDrum = 0x76,
		ReverseCymbal = 0x77,

		// Sound effects
		GuitarFretNoise = 0x78,
		BreathNoise = 0x79,
		Seashore = 0x7A,
		BirdTweet = 0x7B,
		TelephoneRing = 0x7C,
		Helicopter = 0x7D,
		Applause = 0x7E,
		Gunshot = 0x7F
	}

	public static class InstrumentMappings
	{
		// some of these I'm not sure about what they should map to
		readonly public static Dictionary<int, MidiInstrument> LbpToMidiMap = new Dictionary<int, MidiInstrument>
		{
			// 122721 = Keys: Honkytonk Piano
			{ 122721, MidiInstrument.HonkyTonkPiano },
			// 122737 = Keys: Piano
			{ 122737, MidiInstrument.AcousticGrandPiano },

			// 127540 = Percussion: 8-Bit Kit (?)
			{ 127540, MidiInstrument.SynthDrum },
			// 129031 = Percussion: Acoustic Kit 1 (?)
			{ 129031, MidiInstrument.MelodicTom },
			// 148321 = Percussion: Baiyon Kit 1 (?)
			{ 148321, MidiInstrument.SynthDrum },
			// 129040 = Percussion: Beatbox Kit 1 (?)
			{ 129040, MidiInstrument.BreathNoise },
			// 129049 = Percussion: Beatbox Kit 2 (?)
			{ 129049, MidiInstrument.BreathNoise },
			// 129057 = Percussion: Synth Kit 1 (?)
			{ 129057, MidiInstrument.SynthDrum },
			// 129066 = Percussion: Synth Perc. Kit 1 (?)
			{ 129066, MidiInstrument.SynthDrum },

			// 125100 = Plucked: Bass Guitar
			{ 125100, MidiInstrument.FingeredElectricBass },
			// 132205 = Plucked: Electric Guitar (muted)
			{ 132205, MidiInstrument.MutedElectricGuitar },
			// 125101 = Plucked: Electric Guitar Distorted
			{ 125101, MidiInstrument.DistortionGuitar },
			// 138776 = Plucked: Electric Guitar Power Chords
			{ 138776, MidiInstrument.OverdrivenGuitar },
			// 129021 = Plucked: Harp
			{ 129021, MidiInstrument.OrchestralStringsHarp },

			// 143479 = SFX: Baiyon Guildford (?)
			{ 143479, MidiInstrument.Soundtrack },
			// 148320 = SFX: Baiyon Kyoto (?)
			{ 148320, MidiInstrument.Atmosphere },
			// 125386 = SFX: Record Static
			{ 125386, MidiInstrument.BreathNoise },

			// 148318 = Synth: Baiyon Bass 1 (?)
			{ 148318, MidiInstrument.SynthBass1 },
			// 148319 = Synth: Baiyon Bass 2 (?)
			{ 148319, MidiInstrument.SynthBass2 },
			// 148322 = Synth: Baiyon Shiny (?)
			{ 148322, MidiInstrument.Crystal },
			// 148323 = Synth: Baiyon Tinkle (?)
			{ 148323, MidiInstrument.TinkleBell },
			// 129086 = Synth: Bell
			{ 129086, MidiInstrument.TubularBell },
			// 129075 = Synth: E-Piano
			{ 129075, MidiInstrument.ElectricPiano1 },
			// 129076 = Synth: Ghost (?)
			{ 129076, MidiInstrument.HaloPad },
			// 129074 = Synth: Harpsichord
			{ 129074, MidiInstrument.Harpsicord },
			// 129077 = Synth: Mime Artist (?)
			{ 129077, MidiInstrument.Goblins },
			// 129078 = Synth: Mosquito (?)
			{ 129078, MidiInstrument.Whistle },
			// 129079 = Synth: Noise (?)
			{ 129079, MidiInstrument.Brightness },
			// 129080 = Synth: Pulse Wave
			{ 129080, MidiInstrument.SynthVoice },
			// 129081 = Synth: Ray Gun
			{ 129081, MidiInstrument.SciFi },
			// 129082 = Synth: Robot (?)
			{ 129082, MidiInstrument.SynthVoice },
			// 129083 = Synth: Saw Wave
			{ 129083, MidiInstrument.SynthSawWave },
			// 129084 = Synth: Sine Wave
			{ 129084, MidiInstrument.Polysynth },
			// 129015 = Synth: Space Piano (?)
			{ 129015, MidiInstrument.FantasiaNewAge },
			// 129085 = Synth: Square Wave
			{ 129085, MidiInstrument.SynthSquareWave },
			// 129087 = Synth: Strings
			{ 129087, MidiInstrument.SynthStrings1 },
			// 129088 = Synth: Tennis (?)
			{ 129088, MidiInstrument.EchoDropsEchoes },
			// 129089 = Synth: Triangle Wave (?)
			{ 129089, MidiInstrument.MetalPad },
			// 129090 = Synth: Woodpecker (?)
			{ 129090, MidiInstrument.Woodblock },
			// 129091 = Synth: Worm (?)
			{ 129091, MidiInstrument.Atmosphere },

			// 129017 = Tuned Percussion: Glockenspiel
			{ 129017, MidiInstrument.Glockenspiel },

			// 132146 = Wind: Concertina
			{ 132146, MidiInstrument.Accordion },
		};

		public static MidiInstrument GetMidiInstrument(int lbpInstrumentId)
		{
			if (LbpToMidiMap.TryGetValue(lbpInstrumentId, out MidiInstrument midiInstrument))
				return midiInstrument;
			else
			{
				Console.WriteLine($"Warning: No MIDI instrument mapping found for LBP instrument ID {lbpInstrumentId}");
				return MidiInstrument.AcousticGrandPiano;
			}
		}
	}
}
