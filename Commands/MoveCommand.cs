using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TemporalThievery.Commands.Deltas;
using TemporalThievery.Utils;

namespace TemporalThievery.Commands
{
	class MoveCommand : Command
	{
		public override void Execute(PuzzleState puzzle, int arg)
		{
			Point oldPosition = puzzle.Player.Position;
			Point newPosition = DirectionHelper.ShiftPoint(oldPosition, arg);

			if (puzzle.Timelines[puzzle.Player.Timeline].IsWalkableOrPushable(newPosition))
			{
				Element pushable = puzzle.Timelines[puzzle.Player.Timeline].GetPushable(newPosition);

				if (pushable != null)
				{
					deltas.Push(new ElementDelta(puzzle, DirectionHelper.ShiftPoint(pushable.Position, arg), pushable));
				}
			}

			deltas.Push(new PlayerDelta(puzzle, newPosition, puzzle.Player.Timeline));
			base.Execute(puzzle, arg);
		}

		public override void Undo(PuzzleState puzzle)
		{
			base.Undo(puzzle);
		}
	}
}
