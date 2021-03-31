using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery
{
	// Temporal Thievery uses a "command manager" system, in which all actions the player can take are called "commands".
	// The "command manager" class keeps track of all commands the player has executed.
	// All commands can be undone, so by going back down the list of commands, an "undo" feature can be implemented at relatively low cost.

	/// <summary>
	/// Keeps a <see cref="Stack{ICommand}"/> of <see cref="ICommand"/> actions.<br />
	/// Executing the command pushes it to the stack. Undoing the command pops it from the stack.
	/// </summary>
	public class CommandManager
	{
		/// <summary>
		/// A stack of all <see cref="ICommand"/>s executed since the current <see cref="Puzzle"/> began.
		/// </summary>
		public Stack<ICommand> commands;

		/// <summary>
		/// Reference to the current <see cref="Puzzle"/> for <see cref="ICommand"/>s to act on.
		/// </summary>
		public Puzzle puzzle;

		public CommandManager(Puzzle puzzle)
		{
			commands = new Stack<ICommand>();
			this.puzzle = puzzle;
		}

		public void Execute(ICommand command, int arg)
		{
			command.Execute(puzzle, arg);
			commands.Push(command);
			puzzle.Refresh();
		}

		public void Undo()
		{
			if (commands.Count > 0)
				commands.Pop().Undo(puzzle);
			puzzle.Refresh();
		}
	}
}
