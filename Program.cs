using Microsoft.Xna.Framework;
using System;

namespace TemporalThievery
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using var game1 = new Game1();
			{
				game = game1;
				game1.Run();
			}
		}

		public static Game1 game;

		public static Rectangle WindowBounds()
		{
			return game.GraphicsDevice.Viewport.Bounds;
		}
	}
}
