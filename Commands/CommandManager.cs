using System;
using System.Collections.Generic;
using System.Text;
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

		public void Execute(Command command, int arg = 0)
		{
			// Ensure that executing this command will not leave the puzzle in an illegal state.
			PuzzleState tempPuzzle = (PuzzleState)puzzle.Clone();

			command.Execute(tempPuzzle, arg);
			tempPuzzle.Refresh();
			if (tempPuzzle.GetLegality() == PuzzleStateLegality.Legal)
			{
				command.deltas.Clear();
				command.Execute(puzzle, arg);

				// Check for collected cash
				CollectDelta collectDelta = puzzle.CheckPlayerOverlappingMoney();
				if (collectDelta != null)
				{
					command.deltas.Push(collectDelta);
					collectDelta.Execute(puzzle);
				}

				commands.Push(command);



				puzzle.Refresh();
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
