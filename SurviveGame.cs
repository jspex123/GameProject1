using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace GameProject1
{
    public class SurviveGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        private Texture2D background;

        private SurvivorSprite survivor;

        public ZombieSprite[] zombies;

        public SurviveGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            survivor = new SurvivorSprite();
			zombies = new ZombieSprite[]
			{
				new ZombieSprite(){Position = new Vector2(100,200),Direction = Direction.Down},
				new ZombieSprite(){Position = new Vector2(400,250),Direction = Direction.Up},
				new ZombieSprite(){Position = new Vector2(700,200),Direction = Direction.Down}
			};
			base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            background = Content.Load<Texture2D>("background-1");
            survivor.LoadContent(Content);
			foreach (var zombie in zombies)
			{
				zombie.LoadContent(Content);
			}
		}

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            survivor.Update(gameTime);
            survivor.Colliding = false;
            foreach (var zombie in zombies)
            {
                zombie.Update(gameTime);

                if (survivor.Bounds.CollidesWith(zombie.Bounds))
                {
                    survivor.Colliding = true;
                }
            }
			base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
			spriteBatch.Draw(background, new Vector2(-20, 0), Color.White);
			survivor.Draw(gameTime, spriteBatch);
			foreach (var zombie in zombies) zombie.Draw(gameTime, spriteBatch);
			spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
