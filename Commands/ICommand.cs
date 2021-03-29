using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery
{
	/// <summary>
	/// Interface handling the execution of any action the player can take.
	/// </summary>
	public interface ICommand
	{
		/// <summary>
		/// Executes the command on a given <see cref="Puzzle"/>.
		/// </summary>
		/// <param name="puzzle"></param>
		public void Execute(Puzzle puzzle);

		/// <summary>
		/// Undoes the action performed by calling <see cref="Execute(Puzzle)"/>.
		/// </summary>
		/// <param name="puzzle"></param>
		public void Undo(Puzzle puzzle);
	}
}
