using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery.Commands
{
	public class JumpCommand : ICommand
	{
		public void Execute(Puzzle puzzle, int arg)
		{
			throw new NotImplementedException();
		}

		public void Undo(Puzzle puzzle)
		{
			throw new NotImplementedException();
		}
	}
}
