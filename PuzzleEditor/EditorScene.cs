#define WINFORMS

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TemporalThievery.Commands;
using TemporalThievery.Gameplay;
using TemporalThievery.Input;
using TemporalThievery.Loading;
using TemporalThievery.Scenes;

namespace TemporalThievery.PuzzleEditor
{
	internal class EditorScene : Scene
	{
		EditorState state = EditorState.Load;
		PuzzleState editingState;
		// PuzzleState testingState;
		PuzzleScene playtestScene;
		EditorTimelineRenderer renderer;

		Vector3 cursor;

		/// <summary>
		/// Loads the puzzle state into the editor.
		/// </summary>
		/// <param name="puzzleState"></param>
		public void Initialize(PuzzleState puzzleState)
		{
			editingState = (PuzzleState)puzzleState.Clone();
		}

		public void Playtest()
		{
			state = EditorState.Playtest;
			playtestScene = new PuzzleScene();
			playtestScene.InitializePuzzleFromState((PuzzleState)editingState.Clone());
		}

		public void ResetPlaytest()
		{
			playtestScene.InitializePuzzleFromState((PuzzleState)editingState.Clone());
		}

		public void StopPlaytesting()
		{
			state = EditorState.Edit;
		}

		public override void Update(GameTime gameTime)
		{
			switch (state)
			{
				case EditorState.Load:
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
									string json = File.ReadAllText(filePath);
									PuzzleTemplate puzzleLoader = JsonSerializer.Deserialize<PuzzleTemplate>(json);
									editingState = puzzleLoader.ToPuzzle();
									cursor = Vector3.Zero;
									renderer = new EditorTimelineRenderer();
									state = EditorState.Edit;
								}
							}
						}
					}
#endif
					break;
				case EditorState.Edit:
					if (KeyHelper.Pressed(Keys.P))
					{
						Playtest();
					}

					if (KeyHelper.Pressed(Keys.Up))
					{
						if (KeyHelper.Down(Keys.LeftShift) || KeyHelper.Down(Keys.LeftShift))
						{
							SetCursorPosition(cursor.X, cursor.Y, cursor.Z - 1);
						}
						else
						{
							SetCursorPosition(cursor.X, cursor.Y - 1, cursor.Z);
						}
					}
					if (KeyHelper.Pressed(Keys.Down))
					{
						if (KeyHelper.Down(Keys.LeftShift) || KeyHelper.Down(Keys.LeftShift))
						{

							SetCursorPosition(cursor.X, cursor.Y, cursor.Z + 1);
						}
						else
						{
							SetCursorPosition(cursor.X, cursor.Y + 1, cursor.Z);
						}
					}
					if (KeyHelper.Pressed(Keys.Left))
					{
						SetCursorPosition(cursor.X - 1, cursor.Y, cursor.Z);
					}
					if (KeyHelper.Pressed(Keys.Right))
					{
						SetCursorPosition(cursor.X + 1, cursor.Y, cursor.Z);
					}

					if (KeyHelper.Pressed(Keys.W))
					{
						editingState.Timelines[(int)cursor.Z].Layout[(int)cursor.X, (int)cursor.Y] = 0;
					}
					if (KeyHelper.Pressed(Keys.E))
					{
						editingState.Timelines[(int)cursor.Z].Layout[(int)cursor.X, (int)cursor.Y] = 1;
					}
					if (KeyHelper.Pressed(Keys.S))
					{
						editingState.Player.Position = new Point((int)cursor.X, (int)cursor.Y);
						editingState.Player.Timeline = (int)cursor.Z;
					}


					break;
				case EditorState.Playtest:
					if (KeyHelper.Pressed(Keys.R))
					{
						ResetPlaytest();
					}
					if (KeyHelper.Pressed(Keys.P))
					{
						StopPlaytesting();
					}
					playtestScene.Update(gameTime);
					break;
				default:
					break;
			}
			base.Update(gameTime);
		}

		public void SetCursorPosition(float x, float y, float t)
		{
			cursor.X = x;
			cursor.Y = y;
			cursor.Z = t;

			cursor.Z %= editingState.Timelines.Count;
			if (cursor.Z < 0)
			{
				cursor.Z = editingState.Timelines.Count - 1;
			}

			Point dims = editingState.Timelines[(int)cursor.Z].Dimensions;
			cursor.Y = Math.Clamp(cursor.Y, 0, dims.Y - 1);
			cursor.X = Math.Clamp(cursor.X, 0, dims.X - 1);
		}

		public override void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, RenderTarget2D renderTarget)
		{
			switch (state)
			{
				case EditorState.Load:
					spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
					
					spriteBatch.DrawString(Game1.TestFont, "The best part of Temporal Thievery was when he said it's Editing Time and then he edited all over those guys", new Vector2(), Color.White);
					spriteBatch.DrawString(Game1.TestFont, "Press ~ to load a puzzle for editing", new Vector2(0, 10), Color.White);

					spriteBatch.End();
					break;

				case EditorState.Edit:
					spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);

					renderer.DrawState(editingState, spriteBatch, cursor);
					// editingState.Draw(spriteBatch);

					spriteBatch.End();
					break;

				case EditorState.Playtest:
					playtestScene.Draw(gameTime, graphicsDevice, spriteBatch, renderTarget);
					break;

				default:
					break;
			}

			base.Draw(gameTime, graphicsDevice, spriteBatch, renderTarget);
		}
	}

	enum EditorState
	{
		Load,
		Edit,
		Playtest,
	}
}
