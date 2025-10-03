using Microsoft.Xna.Framework;
using GameProject1.StateManagement;

namespace GameProject1.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    public class MainMenuScreen : MenuScreen
    {
		public MainMenuScreen() : base("Survive")
		{
			var playGameMenuEntry = new MenuEntry("Play Game");
			var exitMenuEntry = new MenuEntry("Exit");

			playGameMenuEntry.Selected += OnPlayGameSelected;
			exitMenuEntry.Selected += OnExitSelected;

			MenuEntries.Add(playGameMenuEntry);
			MenuEntries.Add(exitMenuEntry);
		}

		private void OnPlayGameSelected(object sender, PlayerIndexEventArgs e)
		{
			ScreenManager.AddScreen(new GameplayScreen(), e.PlayerIndex);
			ExitScreen();
		}

		private void OnExitSelected(object sender, PlayerIndexEventArgs e)
		{
			ScreenManager.Game.Exit();
		}
	}
}
