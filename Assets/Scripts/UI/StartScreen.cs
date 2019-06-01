using UnityEngine;

public class StartScreen : MonoBehaviour {

	// The player has pressed "quit" button at start menu.
	public void Quit()
	{
		// save the high score...
		// show the credits
		Application.Quit ();
	}

    public void StarGame()
    {
        GameManager.instance.StartGame();
        ScreenManager.instance.ChangeState(ScreenManager.screen.InGame);
    }

    // settings

    // credits
}// end of start script
