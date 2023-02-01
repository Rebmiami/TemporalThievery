using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery
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
				if (element.Type == "Safe")
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
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 origin)
		{
			for (int i = 0; i < Dimensions.X; i++)
			{
				bool checker = i % 2 == 0;
				for (int j = 0; j < Dimensions.Y; j++)
				{
					if (Layout[i, j] == 1)
					{
						spriteBatch.Draw(Game1.GameTiles, origin + new Vector2(i * 8, j * 8), new Rectangle(9 * (checker ? 0 : 1), 9 * 0, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.0f);
					}
					checker = !checker;
				}
			}

			foreach (Element element in Elements)
			{
				switch (element.Type)
				{
					case "Safe":
						{
							spriteBatch.Draw(Game1.GameTiles, origin + element.Position.ToVector2() * 8, new Rectangle(9 * 3, 9 * 0, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.6f);
							break;
						}

					case "Pad":
						{
							spriteBatch.Draw(Game1.GameTiles, origin + element.Position.ToVector2() * 8, new Rectangle(9 * 0, 9 * 1, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.3f);
							break;
						}

					case "Gate":
						{
							bool open = Channels[element.Channel] ^ element.Toggle;
							spriteBatch.Draw(Game1.GameTiles, origin + element.Position.ToVector2() * 8, new Rectangle(9 * (open ? 2 : 1), 9 * 1, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.5f);
							break;
						}

					case "MoneyBag":
						{
							bool open = Channels[element.Channel] ^ element.Toggle;
							spriteBatch.Draw(Game1.GameTiles, origin + element.Position.ToVector2() * 8, new Rectangle(9 * 3, 9 * 1, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.5f);
							break;
						}

					default:
						{
							break;
						}
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
			if (element.Type == "Safe" || element.Type == "Anchor")
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
			if (x < 0 || y < 0 || x >= Dimensions.X || y >= Dimensions.Y)
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

		public Element GetPushable(Point point)
		{
			foreach (Element element in Elements)
			{
				if (element.Position == point && element.Type == "Safe" || element.Type == "Anchor")
				{
					return element;
				}
			}
			return null;
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
