using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery.Commands
{
	public class BranchCommand : ICommand
	{
		/// <summary>
		/// The timeline created by the branch command.
		/// </summary>
		public Timeline CreatedTimeline;

		public void Execute(PuzzleState puzzle, int arg = 0)
		{
			CreatedTimeline = (Timeline)puzzle.Timelines[puzzle.Player.Timeline].Clone();
			puzzle.Timelines.Add(CreatedTimeline);
			puzzle.Branches--;
		}

		public void Undo(PuzzleState puzzle)
		{
			puzzle.Timelines.Remove(CreatedTimeline);
			puzzle.Branches++;
		}
	}
}
