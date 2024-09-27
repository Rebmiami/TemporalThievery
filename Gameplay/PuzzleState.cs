using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using TemporalThievery.Commands.Deltas;
using TemporalThievery.Input;
using TemporalThievery.Utils;

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
		/// Amount of money bags the player has collected.
		/// </summary>
		public int CollectedCash;

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
			bool specialJumpMode = KeyHelper.Down(Keys.X) && Timelines.Count > 2;
			Vector2 origin = new Vector2(60, 15);
			for (int i = 0; i < Timelines.Count; i++)
			{
				Timeline timeline = Timelines[i];
				timeline.DrawDebug(spriteBatch, origin);

				if (Timelines[Player.Timeline] == timeline)
                {
					spriteBatch.Draw(Game1.GameTilesDebug, origin + new Vector2(Player.Position.X * 8, Player.Position.Y * 8), new Rectangle(9 * 2, 9 * 0, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.8f);
				}
				else if (specialJumpMode)
				{
					spriteBatch.DrawString(Game1.TestFont, (i + 1).ToString(), origin + new Vector2(timeline.Dimensions.X * 8 + 3, 0), Color.White);
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

		public PuzzleStateLegality GetLegality(Stack<IDelta> deltas)
		{
			// PlayerDelta is the most important delta, so find the player delta so it can be used later
			// Should it be passed separately?

			// Not all commands produce PlayerDeltas, so this is null by default
			PlayerDelta playerDelta = null;
			foreach (IDelta delta in deltas)
			{
				if (delta is PlayerDelta found)
				{
					playerDelta = found;
				}
			}

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
				return PuzzleStateLegality.PlayerInWall;
			}

			if (playerDelta != null)
			{
				if (playerTimeline.Layout[Player.Position.X, Player.Position.Y] == 2)
				{
					if (playerDelta.newTimeline != playerDelta.oldTimeline)
					{
						return PuzzleStateLegality.Grate;
					}
				}

				// Check if the player is caught within a laser trap
				// Can escape a laser trap by moving to another timeline
				if (playerDelta.newTimeline == playerDelta.oldTimeline)
				{
					foreach (Element element in playerTimeline.Elements)
					{
						if (element.Type == "Laser")
						{
							for (int i = 0; i < element.LaserLength; i++)
							{
								Point laserPoint = element.Position + DirectionHelper.ToPoint(element.Direction) * new Point(i);
								if (laserPoint == playerDelta.oldPosition)
								{
									// Stop! You have violated the law!
									return PuzzleStateLegality.Caught;
								}
							}
						}
					}
				}
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

					if (element.Type == "OneWay")
					{
						foreach (IDelta delta in deltas)
						{
							if (delta is PlayerDelta //playerDelta 
								&& element.Position == Player.Position
								&& playerDelta.direction == DirectionHelper.Invert((Directions)element.Direction))
							{
								return PuzzleStateLegality.PlayerThroughOneWayWall;
							}
							if (delta is ElementDelta elemDelta
								&& element.Position == elemDelta.newPosition
								&& elemDelta.direction == DirectionHelper.Invert((Directions)element.Direction))
							{
								return PuzzleStateLegality.ObjectThroughOneWayWall;
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

			// Innocent until proven guilty
			return PuzzleStateLegality.Legal;
		}

		public CollectDelta CheckPlayerOverlappingMoney()
		{
			int moneyBag = Timelines[Player.Timeline].GetMoneyBag(Player.Position);
			if (moneyBag >= 0)
			{
				return new CollectDelta(this, Player.Timeline, moneyBag);
			}
			return null;
		}

		public object Clone()
		{
			PuzzleState puzzleState = new PuzzleState
			{
				Name = Name,
				CashGoal = CashGoal,
				CollectedCash = CollectedCash,
				Player = (Player)Player.Clone(),
				MaxTimeline = MaxTimeline,
				Timelines = new List<Timeline>(),
				Jumps = Jumps,
				Branches = Branches,
				Returns = Returns,
				Kills = Kills,
				Theme = Theme,
			};
			foreach (Timeline timeline in Timelines)
			{
				puzzleState.Timelines.Add((Timeline)timeline.Clone());
			}
			return puzzleState;
		}
	}
}