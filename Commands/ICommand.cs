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
		/// Executes the command on a given <see cref="PuzzleState"/>.
		/// </summary>
		/// <param name="puzzle"></param>
		/// <param name="arg">The purpose of this argument depends on how the class implenting <see cref="ICommand"/> uses it.</param>
		public void Execute(PuzzleState puzzle, int arg = 0);

		/// <summary>
		/// Undoes the action performed by calling <see cref="Execute(PuzzleState, int)"/>.
		/// </summary>
		/// <param name="puzzle"></param>
		public void Undo(PuzzleState puzzle);
	}
}
