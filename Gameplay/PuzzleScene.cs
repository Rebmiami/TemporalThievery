#define WINFORMS

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using TemporalThievery.Commands;
using TemporalThievery.Input;
using TemporalThievery.Loading;
using TemporalThievery.Scenes;
using TemporalThievery.Utils;

namespace TemporalThievery.Gameplay
{
	public class PuzzleScene : Scene
	{
		public PuzzleState puzzle;
		public CommandManager manager;
		public string puzzlePath;
		public bool editorContext;

		public PuzzleScene()
		{

		}

		public void InitializePuzzleFromFilePath(string path)
		{
			puzzlePath = path;
			string json = File.ReadAllText(path);
			PuzzleTemplate puzzleLoader = JsonSerializer.Deserialize<PuzzleTemplate>(json);
			puzzle = puzzleLoader.ToPuzzle();
			manager = new CommandManager(puzzle);
			puzzle.Refresh();
		}

		public void InitializePuzzleFromState(PuzzleState state)
		{
			// Prevent loading/reloading puzzles since this method does not store a path
			editorContext = true;
			puzzle = state;
			manager = new CommandManager(state);
			state.Refresh();
		}

		public override void Update(GameTime gameTime)
		{
			if (!editorContext)
			{
#if WINFORMS
				if (KeyHelper.Pressed(Keys.OemTilde))
				{
					var fileContent = string.Empty;
					var filePath = string.Empty;

					// TODO: Replace Windows Forms to allow compatibility with other platforms
					using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
					{
						openFileDialog.InitialDirectory = "./Puzzles";
						openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
						openFileDialog.FilterIndex = 2;
						openFileDialog.RestoreDirectory = true;

						if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
						{
							//Get the path of specified file
							filePath = openFileDialog.FileName;

							if (File.Exists(filePath))
							{
								InitializePuzzleFromFilePath(filePath);
							}
						}
					}
				}
#endif
				if (KeyHelper.Pressed(Keys.R))
				{
					InitializePuzzleFromFilePath(puzzlePath);
				}
			}


			// for (int i = 0; i < 10; i++)
			// {
			// 
			// 	if (KeyHelper.Pressed(Keys.D1 + i))
			// 	{
			// 		string path = @".\Puzzles\Chapter_0\Level_" + i + ".json";
			// 		if (File.Exists(path))
			// 		{
			// 			InitializePuzzleFromFilePath(path);
			// 		}
			// 	}
			// }




			// Takes in player input and moves the player avatar accordingly.
			if (KeyHelper.Pressed(Keys.W) || KeyHelper.Pressed(Keys.Up))
			{
				manager.Execute(new MoveCommand(), new int[] { (int)Directions.Up } );
			}
			if (KeyHelper.Pressed(Keys.A) || KeyHelper.Pressed(Keys.Left))
			{
				manager.Execute(new MoveCommand(), new int[] { (int)Directions.Left } );
			}
			if (KeyHelper.Pressed(Keys.S) || KeyHelper.Pressed(Keys.Down))
			{
				manager.Execute(new MoveCommand(), new int[] { (int)Directions.Down } );
			}
			if (KeyHelper.Pressed(Keys.D) || KeyHelper.Pressed(Keys.Right))
			{
				manager.Execute(new MoveCommand(), new int[] { (int)Directions.Right } );
			}


			if (KeyHelper.Down(Keys.X) && puzzle.Jumps != 0)
			{
				if (puzzle.Timelines.Count == 1)
				{

				}
				else if (puzzle.Timelines.Count == 2)
				{
					if (KeyHelper.Pressed(Keys.X))
					{
						manager.Execute(new JumpCommand(), new int[] { 1 - puzzle.Player.Timeline } );
					}
				}
				else
				{
					for (int i = 0; i < 10; i++)
					{
						if (puzzle.Player.Timeline != i && KeyHelper.Pressed(Keys.D1 + i) )
						{
							manager.Execute(new JumpCommand(), new int[] { i } );
							break;
						}
					}
				}
			}

			

			if (KeyHelper.Pressed(Keys.C))
			{
				if (puzzle.Branches != 0)
				{
					manager.Execute(new BranchCommand(), new int[0]);
				}
			}

			if (KeyHelper.Pressed(Keys.Z))
			{
				manager.Undo();
			}
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, RenderTarget2D renderTarget)
		{
			spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);

			spriteBatch.DrawString(Game1.TestFont, puzzle.Name, new Vector2(50, 0), Color.White);

			spriteBatch.Draw(Game1.HUDIcons, new Vector2(4, 10), new Rectangle(0, 0 * 12, 15, 12), Color.White);
			spriteBatch.DrawString(Game1.TestFont, puzzle.Jumps.ToString(), new Vector2(24, 10), Color.White);

			spriteBatch.Draw(Game1.HUDIcons, new Vector2(4, 25), new Rectangle(0, 1 * 12, 15, 12), Color.White);
			spriteBatch.DrawString(Game1.TestFont, puzzle.Branches.ToString(), new Vector2(24, 25), Color.White);

			spriteBatch.Draw(Game1.HUDIcons, new Vector2(4, 40), new Rectangle(0, 2 * 12, 15, 12), Color.White);
			spriteBatch.DrawString(Game1.TestFont, puzzle.Kills.ToString(), new Vector2(24, 40), Color.White);

			spriteBatch.Draw(Game1.HUDIcons, new Vector2(4, 55), new Rectangle(0, 3 * 12, 15, 12), Color.White);
			spriteBatch.DrawString(Game1.TestFont, puzzle.Returns.ToString(), new Vector2(24, 55), Color.White);

			spriteBatch.Draw(Game1.HUDIcons, new Vector2(4, 80), new Rectangle(0, 4 * 12, 15, 12), Color.White);
			spriteBatch.DrawString(Game1.TestFont, puzzle.Timelines.Count + "/" + puzzle.MaxTimeline.ToString(), new Vector2(24, 80), 
				puzzle.Timelines.Count < puzzle.MaxTimeline ? Color.White : Color.Red);

			spriteBatch.Draw(Game1.HUDIcons, new Vector2(4, 95), new Rectangle(0, 5 * 12, 15, 12), Color.White);
			spriteBatch.DrawString(Game1.TestFont, puzzle.CollectedCash + "/" + puzzle.CashGoal.ToString(), new Vector2(24, 95),
				puzzle.CollectedCash < puzzle.CashGoal ? Color.White : Color.Green);





			puzzle.Draw(spriteBatch);
			spriteBatch.End();
			base.Draw(gameTime, graphicsDevice, spriteBatch, renderTarget);
		}
	}
}
