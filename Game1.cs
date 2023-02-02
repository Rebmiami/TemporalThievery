#define WINFORMS

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using TemporalThievery.Commands;
using TemporalThievery.Input;
using TemporalThievery.Utils;

namespace TemporalThievery
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch spriteBatch;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		public static PuzzleState puzzle;
		public static CommandManager manager;

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			InitializePuzzleFromFilePath(@".\Puzzles\Chapter_0\Level_5.json");

			base.Initialize();
		}

		public static Texture2D GameTiles;
		public static Texture2D HUDIcons;
		public static SpriteFont TestFont;

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			GameTiles = Content.Load<Texture2D>("GameTiles");
			HUDIcons = Content.Load<Texture2D>("HUDIcons");
			TestFont = Content.Load<SpriteFont>("TestFont");

			// TODO: use this.Content to load your game content here
		}

		public void InitializePuzzleFromFilePath(string path)
		{
			string json = File.ReadAllText(path);
			PuzzleLoader puzzleLoader = JsonSerializer.Deserialize<PuzzleLoader>(json);
			puzzle = puzzleLoader.ToPuzzle();
			manager = new CommandManager(puzzle);
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();


#if WINFORMS

			if (KeyHelper.Pressed(Keys.OemTilde))
			{
				var fileContent = string.Empty;
				var filePath = string.Empty;
	
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


			for (int i = 0; i < 10; i++)
			{

				if (KeyHelper.Pressed(Keys.D1 + i))
				{
					string path = @".\Puzzles\Chapter_0\Level_" + i + ".json";        
					if (File.Exists(path))
					{
						InitializePuzzleFromFilePath(path);
					}
				}
			}




			// Takes in player input and moves the player avatar accordingly.
			if (KeyHelper.Pressed(Keys.W))
			{
				manager.Execute(new MoveCommand(), (int)Directions.Up);
			}
			if (KeyHelper.Pressed(Keys.A))
			{
				manager.Execute(new MoveCommand(), (int)Directions.Left);
			}
			if (KeyHelper.Pressed(Keys.S))
			{
				manager.Execute(new MoveCommand(), (int)Directions.Down);
			}
			if (KeyHelper.Pressed(Keys.D))
			{
				manager.Execute(new MoveCommand(), (int)Directions.Right);
			}
			if (KeyHelper.Pressed(Keys.X))
			{
				if (puzzle.Jumps != 0)
				{
					manager.Execute(new JumpCommand());
				}
			}
			if (KeyHelper.Pressed(Keys.C))
			{
				if (puzzle.Branches != 0)
				{
					manager.Execute(new BranchCommand());
				}
			}

			if (KeyHelper.Pressed(Keys.Z))
			{
				manager.Undo();
			}

			KeyHelper.Update();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			// TODO: Create a dedicated class for drawing timelines properly
			RenderTarget2D renderTarget = new RenderTarget2D(GraphicsDevice, Program.WindowBounds().Width, Program.WindowBounds().Height);
			GraphicsDevice.SetRenderTarget(renderTarget);

			GraphicsDevice.Clear(new Color(20, 20, 20));
			spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
			puzzle.Draw(spriteBatch);
			spriteBatch.End();
			Program.game.GraphicsDevice.SetRenderTarget(null);

			spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
			spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0.0f);
			spriteBatch.End();
			renderTarget.Dispose();

			base.Draw(gameTime);
		}
	}
}
