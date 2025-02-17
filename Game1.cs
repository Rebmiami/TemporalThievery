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
using Trireme;
using System;
using System.Diagnostics;

namespace TemporalThievery
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch spriteBatch;
		private RendererRoot root;

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

		public GameTime gameTime;

		public void DrawPuzzle(object sender, EventArgs args)
		{
			activeScene.Draw(gameTime, GraphicsDevice, spriteBatch);
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			GameTilesDebug = Content.Load<Texture2D>("GameTiles");
			HUDIcons = Content.Load<Texture2D>("HUDIcons");
			TestFont = Content.Load<SpriteFont>("TestFont");
			EditorCursor = Content.Load<Texture2D>("EditorCursor");

			string json = File.ReadAllText("./Data/Colors.json");
			colors = JsonSerializer.Deserialize<List<Color>>(json);


			root = new RendererRoot(GraphicsDevice, Content);

			string xml = File.ReadAllText("./Trireme/GameScene.xml");
			root.LoadXML(xml);

			root.rootLayer.transformation.scale = new Vector2(2);
			root.rootLayer.transformation.sourceRectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
			root.GetLayerByID("hud").transformation.sourceRectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, 25);
			root.GetLayerByID("game").transformation.sourceRectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height - 25);
			root.GetLayerByID("game").transformation.position.Y = 25;

			(root.GetLayerByID("game") as ManualLayer).ManualDrawEvent += DrawPuzzle;
		}


		public static Scene activeScene;
		public static Scene backgroundScene;

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
			this.gameTime = gameTime;

			root.Draw();
		}
	}
}
