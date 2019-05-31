using UnityEngine;

public class StartScreen : MonoBehaviour {

	public static StartScreen instance;

	//Awake is always called before any Start functions -Unity3D
	void Awake()
	{
		if (instance == null) 								// Check if instance already exists
			instance = this; 								// if not, set instance to "this"
		else if (instance != this)							// If instance already exists and it's not this:
			Destroy(gameObject);    						// Then destroy this. Enforces the singleton pattern.
	}

	// The player has pressed "quit" button at start menu.
	public void Quit()
	{
		// save the high score...
		// show the credits
		print ("player pressed quit. bye!");
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
