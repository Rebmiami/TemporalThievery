using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Text.Json;

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

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			string json = File.ReadAllText(@".\Puzzles\Test.json");
			puzzle = JsonSerializer.Deserialize<Puzzle>(json);

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

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

			puzzle.Draw(spriteBatch);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
