using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TemporalThievery.Gameplay;
using TemporalThievery.Utils;

namespace TemporalThievery.Commands.Deltas
{
	class PlayerDelta : IDelta
	{
		public Point oldPosition;
		public int oldTimeline;

		public Point newPosition;
		public int newTimeline;
		
		public Directions direction;

		public PlayerDelta(PuzzleState puzzle, Point newPos, int newTl, Directions dir)
		{
			Initialize(puzzle);
			newPosition = newPos;
			newTimeline = newTl;
			direction = dir;
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
