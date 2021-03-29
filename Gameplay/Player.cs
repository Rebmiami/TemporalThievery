using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery
{
	public class Player
	{
		/// <summary>
		/// The timeline the player is located within.
		/// </summary>
		public int Timeline;

		/// <summary>
		/// The position of the player relative to the top-left corner of the board.
		/// </summary>
		public Point Position;
	}
}
