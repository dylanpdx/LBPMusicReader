using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBPMusicReader
{
	public class LbpInstrumentComponent
	{
		public float componentX { get; set; }
		public float componentY { get; set; }
		public JToken rawData { get; set; }
	}
}
