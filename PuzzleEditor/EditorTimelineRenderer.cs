using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemporalThievery.Gameplay;
using TemporalThievery.Input;

namespace TemporalThievery.PuzzleEditor
{
	/// <summary>
	/// Renders the puzzle in the editor using simple sprites.
	/// </summary>
	internal class EditorTimelineRenderer
	{
		public void DrawState(PuzzleState state, SpriteBatch spriteBatch, Vector3 cursor)
		{
			bool specialJumpMode = KeyHelper.Down(Keys.X) && state.Timelines.Count > 2;
			Vector2 origin = new Vector2(60, 15);
			for (int i = 0; i < state.Timelines.Count; i++)
			{
				Timeline timeline = state.Timelines[i];
				timeline.DrawDebug(spriteBatch, origin);

				if (state.Timelines[state.Player.Timeline] == timeline)
				{
					spriteBatch.Draw(Game1.GameTilesDebug, origin + new Vector2(state.Player.Position.X * 8, state.Player.Position.Y * 8), new Rectangle(9 * 2, 9 * 0, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.8f);
				}
				else if (specialJumpMode)
				{
					spriteBatch.DrawString(Game1.TestFont, (i + 1).ToString(), origin + new Vector2(timeline.Dimensions.X * 8 + 3, 0), Color.White);
				}

				if (i == cursor.Z)
				{
					spriteBatch.Draw(Game1.EditorCursor, origin + new Vector2(cursor.X * 8, cursor.Y * 8), new Rectangle(0, 0, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
				}


				origin.Y += timeline.Dimensions.Y * 8 + 16;
				if (origin.Y + timeline.Dimensions.Y * 8 > 430 / 2)
				{
					origin.Y = 15;
					origin.X += timeline.Dimensions.X * 8 + 16;
				}
			}
		}
	}
}
