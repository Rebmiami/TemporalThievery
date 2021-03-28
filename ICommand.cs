using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery
{
	public interface ICommand
	{
		public void Execute(Puzzle puzzle);

		public void Undo(Puzzle puzzle);
	}
}
