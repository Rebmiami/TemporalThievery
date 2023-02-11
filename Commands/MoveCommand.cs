using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TemporalThievery.Commands.Deltas;
using TemporalThievery.Gameplay;
using TemporalThievery.Utils;

namespace TemporalThievery.Commands
{
	class MoveCommand : Command
	{
		public override void Execute(PuzzleState puzzle, int[] args)
		{
			Point oldPosition = puzzle.Player.Position;
			Point newPosition = DirectionHelper.ShiftPoint(oldPosition, args[0]);

			deltas.Push(new PlayerDelta(puzzle, newPosition, puzzle.Player.Timeline, (Directions)args[0]));

			if (puzzle.Timelines[puzzle.Player.Timeline].IsWalkableOrPushable(newPosition))
			{
				Element pushable = puzzle.Timelines[puzzle.Player.Timeline].GetPushable(newPosition);

				while (pushable != null)
				{
					if (pushable != null)
					{
						deltas.Push(new ElementDelta(puzzle, DirectionHelper.ShiftPoint(pushable.Position, args[0]), pushable, (Directions)args[0]));
					}
					DirectionHelper.ShiftPoint(ref newPosition, args[0]);
					pushable = puzzle.Timelines[puzzle.Player.Timeline].GetPushable(newPosition);

					if (pushable != null && pushable.Type == "Safe")
					{
						break;
					}
				}
			}
			base.Execute(puzzle, args);
		}

		public override void Undo(PuzzleState puzzle)
		{
			base.Undo(puzzle);
		}
	}
}
