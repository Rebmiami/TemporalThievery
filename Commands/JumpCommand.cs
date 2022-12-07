using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery.Commands
{
	public class JumpCommand : ICommand
	{
		public void Execute(PuzzleState puzzle, int arg)
		{
			puzzle.Player.Timeline++;
			if (puzzle.Player.Timeline >= puzzle.Timelines.Count)
            {
				puzzle.Player.Timeline = 0;
            }
			puzzle.Jumps--;
		}

		public void Undo(PuzzleState puzzle)
		{
			puzzle.Player.Timeline--;
			if (puzzle.Player.Timeline < 0)
			{
				puzzle.Player.Timeline = puzzle.Timelines.Count - 1;
			}
			puzzle.Jumps++;
		}
	}
}
