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

			// Setup up the default resolution for the project
			Point resolution = Window.ClientBounds.Size;
    		_graphics.PreferredBackBufferWidth = resolution.X;
    		_graphics.PreferredBackBufferHeight = resolution.Y;

    		// Runs the game in "full Screen" mode using the set resolution
    		_graphics.IsFullScreen = true;

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

		public void UpdateViewport(object sender, EventArgs eventArgs)
    	{
    	    Layer rootLayer = root.rootLayer;

			// float scale = 2f;
			Vector2 idealScale = GraphicsDevice.Viewport.Bounds.Size.ToVector2() / rootLayer.Bounds.ToVector2();
			float scale = Math.Min(idealScale.X, idealScale.Y);
			scale = (float)Math.Floor(scale);

			rootLayer.transformation.scale = new Vector2(scale);
			int vRes = 360;
			float aspectRatio = 4f / 3f;
			rootLayer.Bounds = new Point((int)(vRes * aspectRatio), vRes);
			rootLayer.transformation.origin = rootLayer.Bounds.ToVector2() / 2;
			rootLayer.transformation.position = GraphicsDevice.Viewport.Bounds.Size.ToVector2() / 2;
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
			root.backgroundColor = new Color(40, 40, 40);

			string xml = File.ReadAllText("./Trireme/GameScene.xml");
			root.LoadXML(xml);

        	Window.AllowUserResizing = true;
			Window.ClientSizeChanged += UpdateViewport;
			

			Layer rootLayer = root.rootLayer;

			UpdateViewport(null, null);

			(rootLayer as RecursiveLayer).backgroundColor = new Color(20, 20, 20);

			(root.GetLayerByID("hud") as BufferLayer).sourceRectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, 25);
			(root.GetLayerByID("game") as BufferLayer).sourceRectangle = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height - 25);
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
