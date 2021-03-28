using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemporalThievery
{
	/// <summary>
	/// A template for loading puzzles from JSON. Contains a method <see cref="ToPuzzle"/> to convert itself to a <see cref="Puzzle"/> in a format the game can use.
	/// </summary>
	public class PuzzleLoader
	{
		public string Name { get; set; }

		public int CashGoal { get; set; }

		public int MaxTimelines { get; set; }

		public int Jump { get; set; }

		public int Branch { get; set; }

		public int Kill { get; set; }

		public int Return { get; set; }

		public string Theme { get; set; }

		public List<TimelineLoader> Timelines { get; set; }

		public PlayerLoader Player { get; set; }

		public class TimelineLoader
		{
			public List<ElementLoader> Elements { get; set; }

			public int[][] Layout { get; set; }

			public class ElementLoader
			{
				public string Type { get; set; }

				public int[] Position { get; set; }

				public int Channel { get; set; }

				public bool Toggle { get; set; }

				public int BindChannel { get; set; }


				public Element ToElement()
				{
					var element = new Element
					{
						Type = Type,
						Position = new Vector2(Position[0], Position[1]),
						Channel = Channel,
						Toggle = Toggle,
						BindChannel = BindChannel
					};
					return element;
				}
			}


			public Timeline ToTimeline()
			{
				var timeline = new Timeline();

				timeline.Elements = new List<Element>();
				foreach (ElementLoader element in Elements)
				{
					timeline.Elements.Add(element.ToElement());
				}

				timeline.dimensions = new Point();
				timeline.dimensions.Y = Layout.Length;
				foreach (int[] vs in Layout)
				{
					timeline.dimensions.X = Math.Max(timeline.dimensions.X, vs.Length);
				}
				timeline.Layout = new int[timeline.dimensions.X, timeline.dimensions.Y];
				for (int i = 0; i < timeline.dimensions.X; i++)
				{
					for (int j = 0; j < timeline.dimensions.Y; j++)
					{
						timeline.Layout[i, j] = Layout[j][i];
					}
				}

				return timeline;
			}
		}

		public class PlayerLoader
		{
			public int Timeline { get; set; }

			public int[] Position { get; set; }


			public Player ToPlayer()
			{
				var player = new Player
				{
					Timeline = Timeline,
					Position = new Vector2(Position[0], Position[1])
				};
				return player;
			}
		}


		public Puzzle ToPuzzle()
		{
			var puzzle = new Puzzle
			{
				Name = Name,
				CashGoal = CashGoal,
				MaxTimelines = MaxTimelines,
				Jump = Jump,
				Branch = Branch,
				Kill = Kill,
				Return = Return,
				Theme = Theme,
				Player = Player.ToPlayer()
			};

			puzzle.Timelines = new List<Timeline>();
			foreach (TimelineLoader timeline in Timelines)
			{
				puzzle.Timelines.Add(timeline.ToTimeline());
			}

			return puzzle;
		}
	}
}
