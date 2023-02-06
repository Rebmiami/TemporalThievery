using System;
using System.Collections.Generic;
using System.Text;
using TemporalThievery.Commands.Deltas;
using TemporalThievery.Gameplay;

namespace TemporalThievery.Commands
{
	public class BranchCommand : Command
	{
		public override void Execute(PuzzleState puzzle, int[] args)
		{
			deltas.Push(new BranchDelta(puzzle));
			puzzle.Branches--;
			base.Execute(puzzle, args);
		}

		public override void Undo(PuzzleState puzzle)
		{
			base.Undo(puzzle);
			puzzle.Branches++;
		}
	}
}
