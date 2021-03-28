﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery
{
	public class Timeline
	{
		public List<Element> Elements;
		public int[,] Layout;
		public Point dimensions;

		public void Draw(SpriteBatch spriteBatch, Vector2 origin)
		{
			for (int i = 0; i < dimensions.X; i++)
			{
				bool checker = i % 2 == 0;
				for (int j = 0; j < dimensions.Y; j++)
				{
					if (Layout[i, j] == 1)
					{
						spriteBatch.Draw(Game1.GameTiles, origin + new Vector2(i * 8, j * 8), new Rectangle(9 * (checker ? 0 : 1), 9 * 0, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.0f);
					}
					checker = !checker;
				}
			}
		}
	}
}
