using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPicker.models {
	public class Properties {
		public string Title { get; set; }
		public string Type { get; set; }
		public bool hasTest { get; set; }

		public Properties() { 
		Title = string.Empty;
			Type = string.Empty;
			hasTest = false;
		}
	}
}
