using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporalThievery.Scenes
{
	public abstract class Scene
	{
		public virtual void Update(GameTime gameTime)
		{

		}

		public virtual void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, RenderTarget2D renderTarget = null)
		{
			
		}
	}
}
