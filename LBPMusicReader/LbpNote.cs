using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBPMusicReader
{
	internal class LbpNote
	{
		public int NoteId { get; set; }
		public int length { get; set; }
		public int startTime { get; set; }
		public int globalStartTime { get; set; }
		public int channelId { get; set; }
	}
}
