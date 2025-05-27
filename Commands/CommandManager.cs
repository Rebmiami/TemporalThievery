using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using TemporalThievery.Commands.Deltas;
using TemporalThievery.Gameplay;

namespace TemporalThievery.Commands
{
	// Temporal Thievery uses a "command manager" system, in which all actions the player can take are called "commands".
	// The "command manager" class keeps track of all commands the player has executed.
	// All commands can be undone, so by going back down the list of commands, an "undo" feature can be implemented at relatively low cost.
	// A command may cause multiple individual changes to the state of a puzzle, thus they can be divided into several "deltas".

	/// <summary>
	/// Keeps a <see cref="Stack{ICommand}"/> of <see cref="Command"/> actions.<br />
	/// Executing the command pushes it to the stack. Undoing the command pops it from the stack.
	/// </summary>
	public class CommandManager
	{
		/// <summary>
		/// A stack of all <see cref="Command"/>s executed since the current <see cref="PuzzleState"/> began.
		/// </summary>
		public Stack<Command> commands;

		/// <summary>
		/// Reference to the current <see cref="PuzzleState"/> for <see cref="Command"/>s to act on.
		/// </summary>
		public PuzzleState puzzle;

		public CommandManager(PuzzleState puzzle)
		{
			commands = new Stack<Command>();
			this.puzzle = puzzle;
		}

		public void Execute(Command command, int[] args)
		{
			// Ensure that executing this command will not leave the puzzle in an illegal state.
			PuzzleState tempPuzzle = (PuzzleState)puzzle.Clone();

			command.Execute(tempPuzzle, args);
			tempPuzzle.Refresh();

			PuzzleStateLegality legality = tempPuzzle.GetLegality(command.deltas);

			if (legality == PuzzleStateLegality.Legal)
			{
				command.deltas.Clear();
				command.Execute(puzzle, args);

				// Check for collected cash
				CollectDelta collectDelta = puzzle.CheckPlayerOverlappingMoney();
				if (collectDelta != null)
				{
					command.deltas.Push(collectDelta);
					collectDelta.Execute(puzzle);
				}

				commands.Push(command);

				puzzle.Refresh();

				// Check for loading zones
				int loadingZone = puzzle.Timelines[puzzle.Player.Timeline].GetFirstElementAtPoint(puzzle.Player.Position, "LoadingZone");
				if (loadingZone != -1)
				{
					string destination = puzzle.Timelines[puzzle.Player.Timeline].Elements[loadingZone].Destination;
					Point destinationPosition = puzzle.Timelines[puzzle.Player.Timeline].Elements[loadingZone].DestinationPosition;
					(Game1.activeScene as PuzzleScene).InitializePuzzleFromFilePath(destination);
					(Game1.activeScene as PuzzleScene).puzzle.Player.Position = destinationPosition;
				}
			}
			else
			{
				Debug.WriteLine("Move could not be executed because: " + legality);
			}
		}

		public void Undo()
		{
			// No legality checking is necessary as the previous state must have been legal to be reached
			if (commands.Count > 0)
				commands.Pop().Undo(puzzle);
			puzzle.Refresh();
		}
	}
}
