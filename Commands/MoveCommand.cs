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
		public Element Pushed = null;

		/// <summary>
		/// A code or money bag picked up by the player, or a code gate opened by the player.
		/// </summary>
		public Element Used = null;

		public void Execute(Puzzle puzzle, int arg)
		{
			Point oldPosition = puzzle.Player.Position;
			Point newPosition = DirectionHelper.ShiftPoint(oldPosition, arg);

			bool moved = false;
			if (puzzle.Timelines[puzzle.Player.Timeline].IsWalkableOrPushable(newPosition))
			{
				Element pushable = puzzle.Timelines[puzzle.Player.Timeline].GetPushable(newPosition);

				if (pushable != null)
				{
					if (puzzle.Timelines[puzzle.Player.Timeline].IsUnwalkableOrPushable(DirectionHelper.ShiftPoint(newPosition, arg)))
					{
						moved = false;
					}
					else
					{
						Pushed = pushable;
						DirectionHelper.ShiftPoint(ref pushable.Position, arg);
						moved = true;
					}
				}
				else
				{
					moved = true;
				}
			}

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

			if (Pushed != null)
			{
				Pushed.Position.X -= PlayerMovement.X;
				Pushed.Position.Y -= PlayerMovement.Y;
			}
		}
	}
}
