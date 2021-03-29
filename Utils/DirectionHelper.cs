using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery.Utils
{
	/// <summary>
	/// Provides some utilities for <see cref="Directions"/>, <see cref="int"/>, and <see cref="Point"/>.
	/// </summary>
	public static class DirectionHelper
	{
		public static Point ToPoint(int direction)
		{
			return ToPoint((Directions)direction);
		}

		public static Point ToPoint(Directions direction) =>
			direction switch
			{
				Directions.Up => new Point(0, -1),
				Directions.Down => new Point(0, 1),
				Directions.Left => new Point(-1, 0),
				Directions.Right => new Point(1, 0),
				_ => throw new ArgumentException("The provided direction is not valid.")
			};

		public static Point ShiftPoint(Point point, Directions direction) => (point.ToVector2() + ToPoint(direction).ToVector2()).ToPoint();

		public static Point ShiftPoint(Point point, int direction) => (point.ToVector2() + ToPoint(direction).ToVector2()).ToPoint();

		public static void ShiftPoint(ref Point point, Directions direction)
		{
			point = ShiftPoint(point, direction);
		}

		public static void ShiftPoint(ref Point point, int direction)
        {
			point = ShiftPoint(point, direction);
        }

		public static Directions Invert(Directions direction)
        {
			return direction switch
			{
				Directions.Up => Directions.Down,
				Directions.Down => Directions.Up,
				Directions.Left => Directions.Right,
				Directions.Right => Directions.Left,
				_ => throw new ArgumentException("The provided direction is not valid.")
			};
        }
	}
}
