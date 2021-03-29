using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery
{
	public class Element
	{
		/// <summary>
		/// The type of element represented by this object.
		/// </summary>
		public string Type;

		/// <summary>
		/// The position of the element relative to the top-left corner of the board.
		/// </summary>
		public Vector2 Position;

		/// <summary>
		/// Channel number used for pads to open gates and codes to open code doors. <br />
		/// Gate and code door channels are independent. Gates and code doors can safely share channels.
		/// </summary>
		public byte Channel;

		/// <summary>
		/// Exclusively used by gates to determine if they should be open or closed by default.
		/// </summary>
		public bool Toggle;

		/// <summary>
		/// Channel number used for objects bound over timelines. <br />
		/// Can be applied to crates and pads. Crates and pads can safely share bind channels.
		/// </summary>
		public byte BindChannel;
	}
}
