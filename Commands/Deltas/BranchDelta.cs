using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TemporalThievery.Gameplay;

namespace TemporalThievery.Commands.Deltas
{
	class BranchDelta : IDelta
	{
		Timeline newTimeline;

		public BranchDelta(PuzzleState puzzle)
		{
			Initialize(puzzle);
		}

		public void Initialize(PuzzleState puzzle, int arg = 0)
		{
			newTimeline = (Timeline)puzzle.Timelines[puzzle.Player.Timeline].Clone();
		}

		public void Execute(PuzzleState puzzle)
		{
			puzzle.Timelines.Add(newTimeline);
		}

		public void Undo(PuzzleState puzzle)
		{
			puzzle.Timelines.Remove(newTimeline);
		}
	}
}
