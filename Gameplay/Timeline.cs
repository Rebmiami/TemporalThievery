﻿using Microsoft.Xna.Framework;
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
		}

		public bool IsWalkable(Point point) => IsWalkable(point.X, point.Y);

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
		// TODO: Expand this method to take into account other non-walkable elements, such as closed gates or safes pushed against a wall.
	}
}
