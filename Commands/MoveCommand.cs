using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TemporalThievery.Utils;

namespace TemporalThievery.Commands
{
	class MoveCommand : ICommand
	{
		public Point PlayerMovement;

		/// <summary>
		/// A safe or anchor pushed by the player on a given move.
		/// </summary>
		public Element Pushed;

		/// <summary>
		/// A code or money bag picked up by the player, or a code gate opened by the player.
		/// </summary>
		public Element Used;

		public void Execute(Puzzle puzzle, int arg)
		{
			Point oldPosition = puzzle.Player.Position;
			Point newPosition = DirectionHelper.ShiftPoint(oldPosition, arg);
			bool moved = puzzle.Timelines[puzzle.Player.Timeline].IsWalkable(newPosition);
			if (moved)
			{
				PlayerMovement = DirectionHelper.ToPoint(arg);
				puzzle.Player.Position = newPosition;
			}
			else
			{
				PlayerMovement = Point.Zero;
			}
		}


		public void Undo(Puzzle puzzle)
		{
			puzzle.Player.Position.X -= PlayerMovement.X;
			puzzle.Player.Position.Y -= PlayerMovement.Y;
		}
	}
}
