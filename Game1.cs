using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Text.Json;
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

		public static Puzzle puzzle;
		public static CommandManager manager;

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			string json = File.ReadAllText(@".\Puzzles\Test.json");
			PuzzleLoader puzzleLoader = JsonSerializer.Deserialize<PuzzleLoader>(json);
			puzzle = puzzleLoader.ToPuzzle();
			manager = new CommandManager(puzzle);

			base.Initialize();
		}

		public static Texture2D GameTiles;
		public static Texture2D HUDIcons;

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			GameTiles = Content.Load<Texture2D>("GameTiles");
			HUDIcons = Content.Load<Texture2D>("HUDIcons");

			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

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
			if (KeyHelper.Pressed(Keys.D1))
			{
				if (puzzle.Jumps != 0)
				{
					manager.Execute(new JumpCommand());
				}
			}
			if (KeyHelper.Pressed(Keys.D2))
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
			// TODO: Create a dedicated class for drawing timelines right
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
