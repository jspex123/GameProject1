using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameProject1.Collisions;

namespace GameProject1
{
	public class SurvivorSprite
	{
		private Texture2D texture;
		private KeyboardState keyboardState;
		private Vector2 position = new Vector2(400, 240);
		public Vector2 Position => position;
		private float rotation;
		private MouseState mouseState;

		private BoundingCircle bounds;

		public BoundingCircle Bounds => bounds;

		public bool Colliding { get; set; }

		public void LoadContent(ContentManager content)
		{
			texture = content.Load<Texture2D>("survivor-idle_handgun_0");
			bounds = new BoundingCircle(position, 45f);
		}

		public void Update(GameTime gameTime, Matrix cameraMatrix)
		{
			keyboardState = Keyboard.GetState();
			mouseState = Mouse.GetState();
			Matrix inverseCamera = Matrix.Invert(cameraMatrix);
			Vector2 mouseScreenPosition = new Vector2(mouseState.X, mouseState.Y);
			Vector2 mouseWorldPosition = Vector2.Transform(mouseScreenPosition, inverseCamera);
			Vector2 direction = mouseWorldPosition - position;
			rotation = (float)Math.Atan2(direction.Y, direction.X);

			if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W)) position += new Vector2(0, -2);
			if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S)) position += new Vector2(0, 2);
			if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A)) position += new Vector2(-2, 0);
			if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D)) position += new Vector2(2, 0);

			bounds.Center = position;
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Vector2 origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

			Color color = Colliding ? Color.Red : Color.White;
			spriteBatch.Draw(texture, position, null, color, rotation, origin, 0.5f, SpriteEffects.None, 0);
		}
	}
}
