using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery.Commands
{
	class Move : ICommand
	{
		public Point playerMovement;

		public void Execute(Puzzle puzzle)
		{
			throw new NotImplementedException();
		}

		public void Undo(Puzzle puzzle)
		{
			throw new NotImplementedException();
		}
	}
}
