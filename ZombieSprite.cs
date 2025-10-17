using GameProject1.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject1
{ 
	public class ZombieSprite
	{
		private Texture2D[] textures;

		private double directionTimer;

		private double animationTimer;

		private short animationFrame;

		private Vector2 position;

		private float rotation;

		private BoundingCircle bounds;
		
		public BoundingCircle Bounds => bounds;

		public float Speed;

		public Vector2 Position
		{
			get => position;
			set => position = value;
		}
		public ZombieSprite(float speed)
		{
			Speed = speed;
		}

		public void LoadContent(ContentManager content)
		{
			textures = new Texture2D[3];
			textures[0] = content.Load<Texture2D>("skeleton-move_0");
			textures[1] = content.Load<Texture2D>("skeleton-move_4");
			textures[2] = content.Load<Texture2D>("skeleton-move_7");
			bounds = new BoundingCircle(Position, 45f);
		}

		public void Update(GameTime gameTime, Vector2 survivorPosition)
		{
			Vector2 direction = survivorPosition - position;
			float distance = direction.Length();

			if (distance > 0)
			{
				direction /= distance;

				position += direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

				rotation = (float)Math.Atan2(direction.Y, direction.X);
			}

			animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
			if (animationTimer > 0.3)
			{
				animationFrame++;
				if (animationFrame > 2) animationFrame = 0;
				animationTimer -= 0.3;
			}

			bounds.Center = Position;
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Vector2 origin = new Vector2(textures[animationFrame].Width / 2f, textures[animationFrame].Height / 2f);
			
			spriteBatch.Draw(textures[animationFrame], position, null, Color.White, rotation, origin, 0.5f, SpriteEffects.None, 0);
		}
	}
}
