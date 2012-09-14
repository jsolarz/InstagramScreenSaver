using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace InstagramScreenSaver.Entities
{
	public class InstaImage
	{
		public string LowResolution { get; set; }
		public string Thumbnail { get; set; }
		public string StandardResolution { get; set; }
	}
}
