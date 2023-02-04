using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TemporalThievery.Gameplay
{
	/// <summary>
	/// Contains information on the current puzzle.
	/// </summary>
	public class PuzzleState : ICloneable
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
		public int MaxTimeline;

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
		// TODO: Move most of the fields above this comment to a new class (PuzzleInfo?)

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
			Vector2 origin = new Vector2(60, 15);
			foreach (Timeline timeline in Timelines)
			{
				timeline.Draw(spriteBatch, origin);

				if (Timelines[Player.Timeline] == timeline)
                {
					spriteBatch.Draw(Game1.GameTiles, origin + new Vector2(Player.Position.X * 8, Player.Position.Y * 8), new Rectangle(9 * 2, 9 * 0, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.8f);
				}

				origin.Y += timeline.Dimensions.Y * 8 + 16;
				if (origin.Y + timeline.Dimensions.Y * 8 > 430 / 2)
				{
					origin.Y = 15;
					origin.X += timeline.Dimensions.X * 8 + 16;
				}
			}
		}

		public void Refresh()
		{
			foreach (Timeline timeline in Timelines)
			{
				timeline.Refresh();
			}
		}

		public PuzzleStateLegality GetLegality()
		{


			if (Timelines.Count == 0)
			{
				return PuzzleStateLegality.NoTimelines;
			}
			if (Timelines.Count > MaxTimeline)
			{
				return PuzzleStateLegality.TooManyTimelines;
			}

			Timeline playerTimeline = Timelines[Player.Timeline];
			if (Player.Position.X < 0 || Player.Position.Y < 0 || Player.Position.X >= playerTimeline.Dimensions.X || Player.Position.Y >= playerTimeline.Dimensions.Y)
			{
				return PuzzleStateLegality.PlayerOutOfBounds;
			}
			if (playerTimeline.Layout[Player.Position.X, Player.Position.Y] == 0)
			{
				return PuzzleStateLegality.PLayerInWall;
			}

			foreach (Timeline timeline in Timelines)
			{
				Element[,] elementsMap = new Element[timeline.Dimensions.X, timeline.Dimensions.Y];
				foreach (Element element in timeline.Elements)
				{
					if (timeline.IsElementSolid(element))
					{
						if (element.Position.X < 0 || element.Position.Y < 0 || element.Position.X >= timeline.Dimensions.X || element.Position.Y >= timeline.Dimensions.Y)
						{
							return PuzzleStateLegality.ObjectOutOfBounds;
						}
						if (elementsMap[element.Position.X, element.Position.Y] != null)
						{
							return PuzzleStateLegality.OverlappingSolidObjects;
						}
						else
						{
							elementsMap[element.Position.X, element.Position.Y] = element;
							if (timeline.Layout[element.Position.X, element.Position.Y] == 0)
							{
								return PuzzleStateLegality.ObjectInWall;
							}
						}
					}
				}
				if (timeline == playerTimeline)
				{
					if (elementsMap[Player.Position.X, Player.Position.Y] != null)
					{
						return PuzzleStateLegality.PlayerInSolidObject;
					}
				}
			}

			return PuzzleStateLegality.Legal;
		}

		public object Clone()
		{
			PuzzleState puzzleState = new PuzzleState
			{
				Player = (Player)Player.Clone(),
				MaxTimeline = MaxTimeline,
				Timelines = new List<Timeline>(),
			};
			foreach (Timeline timeline in Timelines)
			{
				puzzleState.Timelines.Add((Timeline)timeline.Clone());
			}
			return puzzleState;
		}
	}
}