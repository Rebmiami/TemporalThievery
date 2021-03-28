using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery
{
	public class Timeline
	{
		public List<Element> Elements { get; set; }

		public int[][] Layout { get; set; }

		public void Draw(SpriteBatch spriteBatch, Vector2 origin)
		{
			for (int i = 0; i < Layout.Length; i++)
			{
				bool checker = i % 2 == 0;
				for (int j = 0; j < Layout[i].Length; j++)
				{
					if (Layout[i][j] == 1)
                    {
						spriteBatch.Draw(Game1.GameTiles, origin + new Vector2(j * 8, i * 8), new Rectangle(9 * (checker ? 0 : 1), 9 * 0, 8, 8), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.0f);
                    }
					checker = !checker;
				}
			}
		}
	}
}
