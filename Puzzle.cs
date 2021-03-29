using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TemporalThievery
{
	public class Puzzle
	{
		public string name;

		public int cashGoal;
		public int maxTimelines;

		public int jumps;
		public int branches;
		public int kills;
		public int returns;

		public string theme;

		public List<Timeline> timelines;
		public Player player;

		public void Draw(SpriteBatch spriteBatch)
		{
			Vector2 origin = new Vector2(50);
			foreach (Timeline timeline in timelines)
			{
				timeline.Draw(spriteBatch, origin);
			}
			spriteBatch.Draw(Game1.GameTiles, origin + new Vector2(player.Position.X * 8, player.Position.Y * 8), new Rectangle(9 * 2, 9 * 0, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.8f);
		}
	}
}