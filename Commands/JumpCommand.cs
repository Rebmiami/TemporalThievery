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
			int timeline = puzzle.Player.Timeline + 1;
			if (timeline >= puzzle.Timelines.Count)
            {
				timeline = 0;
			}
			deltas.Push(new PlayerDelta(puzzle, puzzle.Player.Position, timeline));
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
