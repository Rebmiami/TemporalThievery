using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery
{
	public class Puzzle
	{
		public string Name { get; set; }

		public int CashGoal { get; set; }

		public int MaxTimelines { get; set; }

		public int Jump { get; set; }

		public int Branch { get; set; }

		public int Kill { get; set; }

		public int Return { get; set; }

		public string Theme { get; set; }

		public Timeline[] Timelines { get; set; }
	}
}
