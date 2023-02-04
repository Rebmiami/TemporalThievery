using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TemporalThievery.Gameplay;

namespace TemporalThievery.Commands.Deltas
{
	class ElementDelta : IDelta
	{
		Element element;
		Point oldPosition;
		Point newPosition;

		public ElementDelta(PuzzleState puzzle, Point newPos, int tl, int elID)
		{
			newPosition = newPos;
			element = puzzle.Timelines[tl].Elements[elID];
			Initialize(puzzle);
		}

		public ElementDelta(PuzzleState puzzle, Point newPos, Element elem)
		{
			newPosition = newPos;
			element = elem;
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
