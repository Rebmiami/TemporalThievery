﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery
{
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

		public void Execute(ICommand command, int arg)
		{
			command.Execute(puzzle, arg);
			commands.Push(command);
		}

		public void Undo()
		{
			if (commands.Count > 0)
				commands.Pop().Undo(puzzle);
		}
	}
}
