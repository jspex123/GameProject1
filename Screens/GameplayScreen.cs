using GameProject1;
using GameProject1.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.TimeZoneInfo;
using Color = Microsoft.Xna.Framework.Color;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace GameProject1.Screens
{
	public class GameplayScreen : GameScreen
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch spriteBatch;
		private Texture2D background;

		private SurvivorSprite survivor;
		private Song backgroundMusic;
		private float hitSoundCooldown = 0f;
		private SoundEffect hitFromZombie;

		public ZombieSprite[] zombies;
		public Matrix matrix;
		private float backgroundScale = 1.5f;
		private Vector2 worldSize;

		public GameplayScreen()
		{
			TransitionOnTime = TimeSpan.FromSeconds(0.5);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);
		}

		public override void Activate()
		{
			var content = ScreenManager.Game.Content;

			survivor = new SurvivorSprite();
			zombies = new ZombieSprite[]
			{
				new ZombieSprite(20f) { Position = new Vector2(100, 200) },
				new ZombieSprite(50f) { Position = new Vector2(200, 150) },
				new ZombieSprite(30f) { Position = new Vector2(700, 200) }
			};

			background = content.Load<Texture2D>("background-1");
			worldSize = new Vector2(background.Width * backgroundScale, background.Height * backgroundScale);
			survivor.LoadContent(content);
			foreach (var zombie in zombies)
			{
				zombie.LoadContent(content);
			}
			hitFromZombie = content.Load<SoundEffect>("pained_noise_01");
			backgroundMusic = content.Load<Song>("ZombiesAreComing");
			MediaPlayer.IsRepeating = true;
			MediaPlayer.Play(backgroundMusic);
		}

		public override void Deactivate()
		{
			MediaPlayer.Stop();
		}

		public override void HandleInput(GameTime gameTime, InputState input)
		{
			PlayerIndex playerIndex;
			if (input.IsNewKeyPress(Keys.Escape, ControllingPlayer, out playerIndex) ||
				input.IsNewButtonPress(Buttons.Back, ControllingPlayer, out playerIndex))
			{
				ScreenManager.AddScreen(new MainMenuScreen(), ControllingPlayer);
				ExitScreen();
			}
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

			if (!IsActive)
				return;

			hitSoundCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

			survivor.Update(gameTime, matrix);
			survivor.Colliding = false;

			bool soundPlayedThisFrame = false;
			foreach (var zombie in zombies)
			{
				zombie.Update(gameTime, survivor.Bounds.Center);
				if (survivor.Bounds.CollidesWith(zombie.Bounds))
				{
					survivor.Colliding = true;
					if (!soundPlayedThisFrame && hitSoundCooldown <= 0)
					{
						hitFromZombie.Play();
						hitSoundCooldown = 0.75f;
						soundPlayedThisFrame = true;
					}
				}
			}
			var viewport = ScreenManager.GraphicsDevice.Viewport;
			Vector2 screenCenter = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
			float cameraX = survivor.Bounds.Center.X;
			float cameraY = survivor.Bounds.Center.Y;

			float minX = viewport.Width / 2f;
			float maxX = worldSize.X - viewport.Width / 2f;
			float minY = viewport.Height / 2f;
			float maxY = worldSize.Y - viewport.Height / 2f;
			cameraX = MathHelper.Clamp(cameraX, minX, maxX);
			cameraY = MathHelper.Clamp(cameraY, minY, maxY);

			matrix = Matrix.CreateTranslation(-cameraX, -cameraY, 0) *
						   Matrix.CreateTranslation(screenCenter.X, screenCenter.Y, 0);
		}

		public override void Draw(GameTime gameTime)
		{
			var spriteBatch = ScreenManager.SpriteBatch;
			spriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: matrix);
			spriteBatch.Draw(background, Vector2.Zero, null, Color.White * TransitionAlpha, 0f, Vector2.Zero, backgroundScale, SpriteEffects.None, 0);
			survivor.Draw(gameTime, spriteBatch);
			foreach (var zombie in zombies)
			{
				zombie.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();
			ScreenManager.FadeBackBufferToBlack(1f - TransitionAlpha);
		}
	}
}
