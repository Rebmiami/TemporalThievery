using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery
{
	public class Player : ICloneable
	{
		/// <summary>
		/// The timeline the player is located within.
		/// </summary>
		public int Timeline;

		/// <summary>
		/// The position of the player relative to the top-left corner of the board.
		/// </summary>
		public Point Position;

		public object Clone()
		{
			return new Player
			{
				Timeline = Timeline,
				Position = Position,
			};
		}

		// As of now, code related to the movement of the player is located in Game1.cs under Update(GameTime gameTime).
	}
}
