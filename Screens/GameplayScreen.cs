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
				new ZombieSprite() { Position = new Vector2(100, 200), Direction = Direction.Down },
				new ZombieSprite() { Position = new Vector2(400, 250), Direction = Direction.Up },
				new ZombieSprite() { Position = new Vector2(700, 200), Direction = Direction.Down }
			};

			background = content.Load<Texture2D>("background-1");
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

			survivor.Update(gameTime);
			survivor.Colliding = false;

			bool soundPlayedThisFrame = false;
			foreach (var zombie in zombies)
			{
				zombie.Update(gameTime);
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
		}

		public override void Draw(GameTime gameTime)
		{
			var spriteBatch = ScreenManager.SpriteBatch;
			spriteBatch.Begin();
			spriteBatch.Draw(background, new Vector2(-20, 0), Microsoft.Xna.Framework.Color.White * TransitionAlpha);
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
