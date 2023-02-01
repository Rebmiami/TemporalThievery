using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery.Commands.Deltas
{
	class PlayerDelta : IDelta
	{
		Point oldPosition;
		int oldTimeline;

		Point newPosition;
		int newTimeline;

		public PlayerDelta(PuzzleState puzzle, Point newPos, int newTl)
		{
			Initialize(puzzle);
			newPosition = newPos;
			newTimeline = newTl;
		}

		public void Initialize(PuzzleState puzzle, int arg = 0)
		{
			oldPosition = puzzle.Player.Position;
			oldTimeline = puzzle.Player.Timeline;
		}

		public void Execute(PuzzleState puzzle)
		{
			puzzle.Player.Position = newPosition;
			puzzle.Player.Timeline = newTimeline;
		}

		public void Undo(PuzzleState puzzle)
		{
			puzzle.Player.Position = oldPosition;
			puzzle.Player.Timeline = oldTimeline;
		}
	}
}
