using System;
using System.Collections.Generic;
using System.Text;
using TemporalThievery.Commands.Deltas;
using TemporalThievery.Gameplay;

namespace TemporalThievery.Commands
{
	public class JumpCommand : Command
	{
		public override void Execute(PuzzleState puzzle, int[] args)
		{
			deltas.Push(new PlayerDelta(puzzle, puzzle.Player.Position, args[0], Utils.Directions.None));
			puzzle.Jumps--;
			base.Execute(puzzle, args);
		}

		public override void Undo(PuzzleState puzzle)
		{
			// puzzle.Player.Timeline--;
			// if (puzzle.Player.Timeline < 0)
			// {
			// 	puzzle.Player.Timeline = puzzle.Timelines.Count - 1;
			// }
			puzzle.Jumps++;
			base.Undo(puzzle);
		}
	}
}
