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
using TemporalThievery.Scenes;
using TemporalThievery.Gameplay;
using TemporalThievery.PuzzleEditor;

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

		protected override void Initialize()
		{
			PuzzleScene newScene = new PuzzleScene();
			newScene.InitializePuzzleFromFilePath("./Puzzles/Chapter_0/Pads And Gates/Just Another Tile Puzzler.json");
			activeScene = newScene;
			// TODO: Add your initialization logic here


			base.Initialize();
		}

		public static Texture2D GameTilesDebug;
		public static Texture2D HUDIcons;
		public static Texture2D EditorCursor;
		public static SpriteFont TestFont;

		public static List<Color> colors;

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			GameTilesDebug = Content.Load<Texture2D>("GameTiles");
			HUDIcons = Content.Load<Texture2D>("HUDIcons");
			TestFont = Content.Load<SpriteFont>("TestFont");
			EditorCursor = Content.Load<Texture2D>("EditorCursor");

			string json = File.ReadAllText("./Data/Colors.json");
			colors = JsonSerializer.Deserialize<List<Color>>(json);

			// TODO: use this.Content to load your game content here
		}


		public static Scene activeScene;

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			activeScene.Update(gameTime);

			if (KeyHelper.Pressed(Keys.F1))
			{
				activeScene = new EditorScene();
			}

			KeyHelper.Update();
			MouseHelper.Update();
			GamePadHelper.Update();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			// TODO: Create a dedicated class for drawing timelines properly
			RenderTarget2D renderTarget = new RenderTarget2D(GraphicsDevice, Program.WindowBounds().Width, Program.WindowBounds().Height);
			GraphicsDevice.SetRenderTarget(renderTarget);
			GraphicsDevice.Clear(new Color(20, 20, 20));

			activeScene.Draw(gameTime, GraphicsDevice, spriteBatch, renderTarget);

			Program.game.GraphicsDevice.SetRenderTarget(null);

			spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
			spriteBatch.Draw(renderTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0.0f);
			spriteBatch.End();

			renderTarget.Dispose();

			base.Draw(gameTime);
		}
	}
}
