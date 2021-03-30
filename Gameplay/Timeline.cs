using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery
{
	public class Timeline
	{
		/// <summary>
		/// A list of elements on the timeline's board.
		/// </summary>
		public List<Element> Elements;

		/// <summary>
		/// The layout of the timeline's board, containing information on which tiles are solid.
		/// </summary>
		public int[,] Layout;

		/// <summary>
		/// The dimensions of the board.
		/// </summary>
		public Point Dimensions;

		/// <summary>
		/// All 256 channels used by pads and gates. Usually, no more than 5-10 will be used.
		/// </summary>
		public bool[] Channels;

		public Timeline()
		{
			Channels = new bool[byte.MaxValue];
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
			try
			{
				return Layout[x, y] != 0;
			}
			catch (IndexOutOfRangeException)
			{
				return false;
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
	}
}
