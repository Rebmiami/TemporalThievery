using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery
{
	public class Element
	{
		public string Type { get; set; }

		public int[] Position { get; set; }

		public int Channel { get; set; }

		public bool Toggle { get; set; }

		public int BindChannel { get; set; }
	}
}
