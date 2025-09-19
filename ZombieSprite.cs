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
	public enum Direction
	{
		Down = 0,
		Up = 2,
	}
	public class ZombieSprite
	{
		private Texture2D[] textures;

		private double directionTimer;

		private double animationTimer;

		private short animationFrame;

		public Direction Direction;

		public Vector2 Position;

		private BoundingCircle bounds;
		
		public BoundingCircle Bounds => bounds;

		public void LoadContent(ContentManager content)
		{
			textures = new Texture2D[3];
			textures[0] = content.Load<Texture2D>("skeleton-move_0");
			textures[1] = content.Load<Texture2D>("skeleton-move_4");
			textures[2] = content.Load<Texture2D>("skeleton-move_7");
			bounds = new BoundingCircle(Position, 45f);
		}

		public void Update(GameTime gameTime)
		{
			directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

			if (directionTimer > 2.0)
			{
				Direction = Direction == Direction.Up ? Direction.Down : Direction.Up;
				directionTimer -= 2.0;
			}
			switch (Direction)
			{
				case Direction.Up:
					Position += new Vector2(0, -1) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
					break;
				case Direction.Down:
					Position += new Vector2(0, 1) * 100 * (float)gameTime.ElapsedGameTime.TotalSeconds;
					break;
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
			float rotation = Direction == Direction.Up ? -MathHelper.PiOver2 : MathHelper.PiOver2;
			
			Vector2 origin = new Vector2(textures[animationFrame].Width / 2f, textures[animationFrame].Height / 2f);
			
			spriteBatch.Draw(textures[animationFrame], Position, null, Color.White, rotation, origin, 0.5f, SpriteEffects.None, 0);
		}
	}
}
