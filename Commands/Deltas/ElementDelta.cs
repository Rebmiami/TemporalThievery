using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TemporalThievery.Gameplay;
using TemporalThievery.Utils;

namespace TemporalThievery.Commands.Deltas
{
	class ElementDelta : IDelta
	{
		public Element element;
		public Point oldPosition;
		public Point newPosition;

		public Directions direction;

		public ElementDelta(PuzzleState puzzle, Point newPos, int tl, int elID, Directions dir)
		{
			newPosition = newPos;
			element = puzzle.Timelines[tl].Elements[elID];
			direction = dir;
			Initialize(puzzle);
		}

		public ElementDelta(PuzzleState puzzle, Point newPos, Element elem, Directions dir)
		{
			newPosition = newPos;
			element = elem;
			direction = dir;
			Initialize(puzzle);
		}

		public void Initialize(PuzzleState puzzle, int arg = 0)
		{
			oldPosition = element.Position;
		}

		public void Execute(PuzzleState puzzle)
		{
			element.Position = newPosition;
		}

		public void Undo(PuzzleState puzzle)
		{
			element.Position = oldPosition;
		}
	}
}
