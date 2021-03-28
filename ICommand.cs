using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery
{
	public interface ICommand
	{
		public void Execute();

		public void Undo();
	}
}
