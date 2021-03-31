using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TemporalThievery
{
	/// <summary>
	/// Contains information on the current puzzle.
	/// </summary>
	public class Puzzle
	{
		/// <summary>
		/// The external name of the puzzle shown to the player.
		/// </summary>
		public string Name;

		/// <summary>
		/// The number of money bags the player must collect to complete the puzzle.
		/// </summary>
		public int CashGoal;

		/// <summary>
		/// The maximum number of timelines the player is allowed to have running parallel.
		/// </summary>
		public int MaxTimelines;

		/// <summary>
		/// The number of "jump" actions the player is allowed to execute.
		/// </summary>
		public int Jumps;

		/// <summary>
		/// The number of "branch" actions the player is allowed to execute.
		/// </summary>
		public int Branches;

		/// <summary>
		/// The number of "kill" actions the player is allowed to execute.
		/// </summary>
		public int Kills;

		/// <summary>
		/// The number of "return" actions the player is allowed to execute.
		/// </summary>
		public int Returns;

		/// <summary>
		/// The aesthetic theme of the puzzle. This affects graphics and sound but has no impact on gameplay.
		/// </summary>
		public string Theme;

		/// <summary>
		/// All active timelines within the puzzle.
		/// </summary>
		public List<Timeline> Timelines;

		/// <summary>
		/// The player's avatar.
		/// </summary>
		public Player Player;

		public void Draw(SpriteBatch spriteBatch)
		{
			Vector2 origin = new Vector2(50);
			foreach (Timeline timeline in Timelines)
			{
				timeline.Draw(spriteBatch, origin);
				origin.Y += timeline.Dimensions.Y * 8 + 16;
			}
			spriteBatch.Draw(Game1.GameTiles, origin + new Vector2(Player.Position.X * 8, Player.Position.Y * 8), new Rectangle(9 * 2, 9 * 0, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.8f);
		}

		public void Refresh()
		{
			foreach (Timeline timeline in Timelines)
			{
				timeline.Refresh();
			}
		}
	}
}