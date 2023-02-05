using System;
using System.Collections.Generic;
using System.Text;
using TemporalThievery.Commands.Deltas;
using TemporalThievery.Gameplay;

namespace TemporalThievery.Commands
{
	public class JumpCommand : Command
	{
		public override void Execute(PuzzleState puzzle, int arg)
		{
			deltas.Push(new PlayerDelta(puzzle, puzzle.Player.Position, arg));
			puzzle.Jumps--;
			base.Execute(puzzle, arg);
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
