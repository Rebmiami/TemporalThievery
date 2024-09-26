using System;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TemporalThievery.Input;
using TemporalThievery.Loading;
using TemporalThievery.Scenes;

namespace TemporalThievery.Utils
{
	internal class FileBrowserScene : Scene
	{
        internal FileBrowser browser;

        internal bool finished = false;
        internal string chosenFilePath;

        public FileBrowserScene(FileBrowser browser)
        {
            this.browser = browser;
        }

        public override void Update(GameTime gameTime)
		{
            
			if (KeyHelper.Pressed(Keys.Left) || KeyHelper.Pressed(Keys.A))
			{
				browser.MoveUpDirectory();
			}
            if (browser.currentDirItems.Count > 0)
            {
                if ((KeyHelper.Pressed(Keys.Right) || KeyHelper.Pressed(Keys.D) || KeyHelper.Pressed(Keys.Enter)) && browser.currentDirItems.Count > 0)
			    {
			    	try
			    	{
			    		if (browser.IsSelectedItemDirectory())
			    		{
			    			browser.OpenSelectedDirectory();
			    		}
			    		else
			    		{
			    			chosenFilePath = browser.OpenSelectedFile();
                            finished = true;
			    		}
			    	}
			    	catch (InvalidOperationException) { }
			    }
			    else if (KeyHelper.Pressed(Keys.Up) || KeyHelper.Pressed(Keys.W))
			    {
			    	browser.MoveCursorUp();
			    }
			    else if (KeyHelper.Pressed(Keys.Down) || KeyHelper.Pressed(Keys.S))
			    {
			    	browser.MoveCursorDown();
			    }
            }
		}

		public override void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, RenderTarget2D renderTarget)
		{
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);

			spriteBatch.DrawString(Game1.TestFont, "Use arrow keys to navigate file selection", new Vector2(0, 20), Color.White);
			spriteBatch.DrawString(Game1.TestFont, "Current folder: " + browser.currentDir, new Vector2(0, 30), Color.White);

			for (int i = 0; i < browser.currentDirItems.Count; i++)
			{
				bool isDirectory = i < browser.subDirectoryCount;
				bool selected = i == browser.cursor;

				int indentation = 20;

				Color color = isDirectory ? Color.Yellow : Color.White;

				if (selected)
					indentation -= (int)Game1.TestFont.MeasureString("> ").X;

				spriteBatch.DrawString(Game1.TestFont, (selected ? "> " : "") + Path.GetFileName(browser.currentDirItems[i]), new Vector2(indentation, 40 + i * 10), color);
			}

            spriteBatch.End();
		}
    }
}