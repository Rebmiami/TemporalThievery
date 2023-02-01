using System;
using System.Collections.Generic;
using System.Text;
using TemporalThievery.Commands.Deltas;

namespace TemporalThievery.Commands
{
	public class BranchCommand : Command
	{
		public override void Execute(PuzzleState puzzle, int arg = 0)
		{
			deltas.Push(new BranchDelta(puzzle));
			puzzle.Branches--;
			base.Execute(puzzle, arg);
		}

		public override void Undo(PuzzleState puzzle)
		{
			base.Undo(puzzle);
			puzzle.Branches++;
		}
	}
}
