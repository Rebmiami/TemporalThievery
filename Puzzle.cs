using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

		public List<Timeline> Timelines { get; set; }


		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Timeline timeline in Timelines)
			{
				timeline.Draw(spriteBatch, new Vector2(50));
			}
		}
	}
}