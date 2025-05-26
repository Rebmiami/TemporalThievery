using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using TemporalThievery.Utils;

namespace TemporalThievery.Gameplay
{
	public class Timeline : ICloneable
	{
		/// <summary>
		/// A list of elements on the timeline's board.
		/// </summary>
		public List<Element> Elements;

		/// <summary>
		/// The layout of the timeline's board, containing information on which tiles are solid.
		/// 0 - Solid tile.
		/// 1 - Passable tile.
		/// </summary>
		public int[,] Layout;

		/// <summary>
		/// The dimensions of the board.
		/// </summary>
		public Point Dimensions;

		/// <summary>
		/// All 256 channels used by pads and gates. Most puzzles will use between 0 and 5, but there is no hard limit below 256.
		/// </summary>
		public bool[] Channels;

		public Timeline()
		{
			Channels = new bool[byte.MaxValue];
		}

		/// <summary>
		/// After the player takes an action, <see cref="Refresh"/> is called to update the status of elements like pads and gates.
		/// </summary>
		public void Refresh()
		{
			// Clears all active channels.
			for (int i = 0; i < Channels.Length; i++)
			{
				Channels[i] = false;
			}

			// Creates a list of all pads in the timeline.
			// 
			List<Element> pads = new List<Element>();
			foreach (Element element in Elements)
			{
				if (element.Type == "Pad")
				{
					pads.Add(element);
				}
			}

			// Checks for safes on top of pads.
			foreach (Element element in Elements)
			{
				// Makes sure that only safes are checked.
				// Should anchors also be checked?
				if (element.Type == "Safe" || element.Type == "Crate")
					foreach (Element pad in pads)
					{
						// If the pad's channel is already active, there is no need to check again.
						// This can only happen if there are multiple pads of the same channel present in the level.
						if (Channels[pad.Channel])
						{
							continue;
						}

						// If the safe is on top of the pad, activate the pad's channel.
						if (element.Position == pad.Position)
						{
							Channels[pad.Channel] = true;

							if (pad.BindChannel != 0)
							{
								// TBA
								// This will cause pads bound between timelines to activate simultaneously.
							}
						}
					}
			}

			foreach (Element element in Elements)
			{
				if (element.Type == "Laser")
				{
					element.LaserLength = 0;


					for (int i = 0; i < 100; i++) // Do`n't ininite loop
					{
						Point laserPoint = element.Position + DirectionHelper.ToPoint(element.Direction) * new Point(element.LaserLength);

						foreach (Element obstacle in Elements)
						{
							if (obstacle.Position == laserPoint)
							{
								if (IsElementSolid(obstacle))
								{
									goto HitObstacle;
								}
								if (obstacle.Type == "OneWay" && obstacle.Direction == (int)DirectionHelper.Invert((Directions)element.Direction))
								{
									goto HitObstacle;
								}
							}
							if (!InBounds(laserPoint) || Layout[laserPoint.X, laserPoint.Y] == 0)
							{
								goto HitObstacle;
							}
						}
						element.LaserLength++;
					}
					HitObstacle:;

				}
			}
		}

		public void DrawDebug(SpriteBatch spriteBatch, Vector2 origin)
		{
			for (int i = 0; i < Dimensions.X; i++)
			{
				bool checker = i % 2 == 0;
				for (int j = 0; j < Dimensions.Y; j++)
				{
					if (Layout[i, j] != 0)
					{
						Vector2 variantOffset = Layout[i, j] switch
						{
							1 => new Vector2(0, 0),
							2 => new Vector2(5, 0),
							_ => throw new Exception("Invalid tile type")
						};
						spriteBatch.Draw(Game1.GameTilesDebug, origin + new Vector2(i * 8, j * 8), new Rectangle(9 * (checker ? 0 : 1) + (int)variantOffset.X * 9, 9 * 0 + (int)variantOffset.Y * 9, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.0f);
					}
					checker = !checker;
				}
			}

			foreach (Element element in Elements)
			{
				DrawElementDebug(spriteBatch, origin + element.Position.ToVector2() * 8, element);
			}
		}

		public void DrawElementDebug(SpriteBatch spriteBatch, Vector2 origin, Element element)
		{
			switch (element.Type)
			{
				case "Safe":
					{
						spriteBatch.Draw(Game1.GameTilesDebug, origin, new Rectangle(9 * 4, 9 * 0, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.6f);
						break;
					}

				case "Pad":
					{
						int channel = element.Channel;
						spriteBatch.Draw(Game1.GameTilesDebug, origin, new Rectangle(9 * 0, 9 * 1, 8, 8), Game1.colors[channel + 2], 0, Vector2.Zero, 1, SpriteEffects.None, 0.3f);
						break;
					}

				case "Gate":
					{
						int channel = element.Channel;
						bool open = Channels[channel] ^ element.Toggle;
						spriteBatch.Draw(Game1.GameTilesDebug, origin, new Rectangle(9 * (open ? 1 : 0), 9 * 2, 8, 8), Game1.colors[channel + 2], 0, Vector2.Zero, 1, SpriteEffects.None, 0.7f);
						break;
					}

				case "MoneyBag":
					{
						bool open = Channels[element.Channel] ^ element.Toggle;
						spriteBatch.Draw(Game1.GameTilesDebug, origin, new Rectangle(9 * 1, 9 * 1, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.5f);
						break;
					}

				case "OneWay":
					{
						int direction = element.Direction;
						spriteBatch.Draw(Game1.GameTilesDebug, origin, new Rectangle(9 * direction, 9 * 3, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.4f);
						break;
					}

				case "Laser":
					{
						int direction = element.Direction;
						int laserLength = element.LaserLength;
						spriteBatch.Draw(Game1.GameTilesDebug, origin, new Rectangle(9 * direction, 9 * 4, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1.0f);
						for (int i = 0; i < laserLength; i++)
						{
							spriteBatch.Draw(Game1.GameTilesDebug, origin + DirectionHelper.ToPoint(direction).ToVector2() * i * 8, new Rectangle(9 * direction, 9 * 5, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
						}
						break;
					}

				case "Crate":
					{
						spriteBatch.Draw(Game1.GameTilesDebug, origin, new Rectangle(9 * 3, 9 * 0, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.6f);
						break;
					}

				default:
					{
						break;
					}
			}
		}

		/// <summary>
		/// Returns true if the element is solid i.e. if it will cause an invalid puzzle state if it overlaps with another solid.
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public bool IsElementSolid(Element element)
		{
			if (element.Type == "Gate" && !Channels[element.Channel] ^ element.Toggle)
			{
				return true;
			}
			if (element.Type == "Safe" || element.Type == "Anchor" || element.Type == "Crate")
			{
				return true;
			}
			return false;
		}

		// Points and pairs of ints are effectively the same thing.

		/// <summary>
		/// Returns the value of <see cref="IsWalkable(int, int)"/> using a <see cref="Point"/> instead of two integers.
		/// </summary>
		/// <param name="point"></param>
		/// <returns></returns>
		public bool IsWalkable(Point point) => IsWalkable(point.X, point.Y);

		/// <summary>
		/// Returns whether or not a given tile on the game board is "walkable". <br />
		/// Unwalkable tiles cannot be traversed by the player under any circumstances.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool IsWalkable(int x, int y)
		{
			// You are never allowed to leave the board.
			if (!InBounds(x, y))
			{
				return false;
			}
			try
			{
				foreach (Element element in Elements)
				{
					if (element.Position == new Point(x, y) && element.Type == "Gate" && !Channels[element.Channel] ^ element.Toggle)
					{
						return false;
					}
				}
				return Layout[x, y] != 0;
			}
			catch (IndexOutOfRangeException)
			{
				throw new Exception("Despite my assumptions, it turns out you CAN index out of bounds here.");
				// return false;
			}
		}

		public bool IsWalkableOrPushable(Point point) => IsWalkableOrPushable(point.X, point.Y);

		/// <summary>
		/// Returns whether or not a given tile on the game board "could be navigable". <br />
		/// All walkable and pushable tiles will return true.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool IsWalkableOrPushable(int x, int y)
		{
			return IsWalkable(x, y) || GetPushable(x, y) != null;
		}

		public bool IsUnwalkableOrPushable(Point point) => IsUnwalkableOrPushable(point.X, point.Y);

		/// <summary>
		/// Returns whether or not a given tile on the game board "could be innavigable". <br />
		/// All unwalkable and pushable tiles will return true.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool IsUnwalkableOrPushable(int x, int y)
		{
			return !IsWalkable(x, y) || GetPushable(x, y) != null;
		}

		public bool InBounds(Point point) => InBounds(point.X, point.Y);

		public bool InBounds(int x, int y)
		{
			return x >= 0 && y >= 0 && x < Dimensions.X && y < Dimensions.Y;
		}

		public Element GetPushable(Point point)
		{
			foreach (Element element in Elements)
			{
				if (element.Position == point && (element.Type == "Safe" || element.Type == "Anchor" || element.Type == "Crate"))
				{
					return element;
				}
			}
			return null;
		}

		public int GetMoneyBag(Point point)
		{
			for (int i = 0; i < Elements.Count; i++)
			{
				Element element = Elements[i];
				if (element.Position == point && element.Type == "MoneyBag")
				{
					return i;
				}
			}
			return -1;
		}

		public Element GetPushable(int x, int y) => GetPushable(new Point(x, y));


		public object Clone()
		{
			Timeline timeline = new Timeline
			{
				Elements = new List<Element>(),
				Layout = (int[,])Layout.Clone(),
				Dimensions = Dimensions,
				Channels = (bool[])Channels.Clone()
			};
			foreach (Element element in Elements)
            {
				timeline.Elements.Add((Element)element.Clone());
            }
			return timeline;
		}
	}
}
