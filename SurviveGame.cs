using GameProject1.Screens;
using GameProject1;
using GameProject1.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace GameProject1
{
    public class SurviveGame : Game
    {
		private readonly GraphicsDeviceManager _graphics;
		private readonly ScreenManager _screenManager;

		public SurviveGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			_screenManager = new ScreenManager(this);
			Components.Add(_screenManager);
		}

		protected override void Initialize()
		{
			base.Initialize();
			_screenManager.AddScreen(new MainMenuScreen(), null);
		}
	}
}
