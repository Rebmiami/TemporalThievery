using System;
using System.Collections.Generic;
using System.Text;
using TemporalThievery.Commands.Deltas;

namespace TemporalThievery
{
	/// <summary>
	/// Interface handling the execution of any action the player can take.
	/// </summary>
	public abstract class Command
	{

		public Stack<IDelta> deltas = new Stack<IDelta>();

		/// <summary>
		/// Executes the command on a given <see cref="PuzzleState"/>.
		/// </summary>
		/// <param name="puzzle"></param>
		/// <param name="arg">The purpose of this argument depends on how the class implenting <see cref="Command"/> uses it.</param>
		public virtual void Execute(PuzzleState puzzle, int arg = 0)
		{
			// Make sure to call base.Execute(puzzle, arg) so that deltas are executed
			foreach (IDelta delta in deltas)
			{
				Console.WriteLine(delta.GetType().ToString());
				delta.Execute(puzzle);
			}
		}

		/// <summary>
		/// Undoes the action performed by calling <see cref="Execute(PuzzleState, int)"/>.
		/// </summary>
		/// <param name="puzzle"></param>
		public virtual void Undo(PuzzleState puzzle)
		{
			// Make sure to call base.Undo(puzzle) so that deltas are undone
			foreach (IDelta delta in deltas)
			{
				Console.WriteLine(delta.GetType().ToString());
				delta.Undo(puzzle);
			}
		}
	}
}
