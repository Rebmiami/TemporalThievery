using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TemporalThievery.Gameplay;

namespace TemporalThievery.Commands.Deltas
{
	/// <summary>
	/// This delta represents the act of the player grabbing a money bag.
	/// </summary>
	public class CollectDelta : IDelta
	{
		public int timeline;
		public int elementID;
		Element element;

		public CollectDelta(PuzzleState puzzle, int tl, int elID)
		{
			timeline = tl;
			elementID = elID;
			element = puzzle.Timelines[tl].Elements[elID];
			Initialize(puzzle);
		}

		// public CollectDelta(PuzzleState puzzle, Element elem)
		// {
		// 	element = elem;
		// 	Initialize(puzzle);
		// }

		public void Initialize(PuzzleState puzzle, int arg = 0)
		{

		}

		public void Execute(PuzzleState puzzle)
		{
			puzzle.Timelines[timeline].Elements.Remove(element);
			puzzle.CollectedCash++;
		}

		public void Undo(PuzzleState puzzle)
		{
			puzzle.Timelines[timeline].Elements.Insert(elementID, element);
			puzzle.CollectedCash--;
		}
	}
}
