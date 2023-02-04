using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemporalThievery.PuzzleEditor
{
	public class PuzzleEditor
	{
		bool editing = true;
		PuzzleState editingState;
		PuzzleState testingState;

		/// <summary>
		/// Loads the puzzle state into the editor.
		/// </summary>
		/// <param name="puzzleState"></param>
		public void Initialize(PuzzleState puzzleState)
		{
			editingState = (PuzzleState)puzzleState.Clone();
		}

		public void Reset()
		{
			
		}

		public void Playtest()
		{

		}
	}
}
