using System;
using System.Collections.Generic;
using System.Text;
using TemporalThievery.Gameplay;

namespace TemporalThievery.Commands.Deltas
{
	/// <summary>
	/// A delta is essentially the atom of a <see cref="Command"/>. <br />
	/// They describe individual actions of the player on the puzzle and contain information on how they can be executed or undone. <br />
	/// A command may contain/cause more than one delta. Deltas should contain no conditional logic.
	/// </summary>
	public interface IDelta
	{
		/// <summary>
		/// Records information about the old state of the puzzle before the delta is executed.
		/// </summary>
		/// <param name="puzzle">The puzzle state.</param>
		/// <param name="arg">Varies depending on type. Usually an index into a list.</param>
		public void Initialize(PuzzleState puzzle, int arg = 0);

		/// <summary>
		/// Executes the delta on a given <see cref="PuzzleState"/>.
		/// </summary>
		/// <param name="puzzle">The puzzle state.</param>
		public void Execute(PuzzleState puzzle);

		/// <summary>
		/// Undoes the action performed by calling <see cref="Execute(PuzzleState)"/>.
		/// </summary>
		/// <param name="puzzle">The puzzle state.</param>
		public void Undo(PuzzleState puzzle);
	}
}
