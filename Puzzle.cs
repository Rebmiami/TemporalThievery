using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TemporalThievery
{
	public class Puzzle
	{
		public string Name;

		public int CashGoal;
		public int MaxTimelines;

		public int Jump;
		public int Branch;
		public int Kill;
		public int Return;

		public string Theme;

		public List<Timeline> Timelines;
		public Player Player;

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Timeline timeline in Timelines)
			{
				timeline.Draw(spriteBatch, new Vector2(50));
			}
		}
	}
}