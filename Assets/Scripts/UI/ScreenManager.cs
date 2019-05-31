using System.Collections.Generic;
using UnityEngine;

// This script controls the UISystem Game object so that correct UI panels are loaded at the right times.
public class ScreenManager : MonoBehaviour {

	// Static Singleton Thing
	public static ScreenManager instance;		// singleton instance.

	// List of UI Panels
	public enum screen { Invalid, Start, InGame, GameOver };

    Dictionary<screen, GameObject> screens;

	// Variables
    [SerializeField]
	private screen current;					// Not accessible from outside. encapsulation etc.
	void Awake ()
	{
		if (instance == null) 			// Check if instance already exists
			instance = this; 			// if not, set instance to "this"
		else if (instance != this)		// If instance already exists and it's not this:
			Destroy (gameObject);    		// Then destroy this. Enforces the singleton pattern. ??? intance or game obj? efeee

		DontDestroyOnLoad (instance); 	//Sets this to not be destroyed when reloading scene
	}

	// Use this for initialization
	void Start () {
        screens = new Dictionary<screen, GameObject>();
        screens.Add(screen.Start, transform.Find("Start").gameObject);
        screens.Add(screen.InGame, transform.Find("InGame").gameObject);
        screens.Add(screen.GameOver, transform.Find("GameOver").gameObject);
        ChangeState(screen.Start);			// Begin at "start" state.
	}//end of start

	// Call this function from outside in order to change the UI state.
	public void ChangeState(screen newState)
	{
		// if we are already at this state, return.
		if (current == newState) 
		{
			print ("you are already at " + newState.ToString () + " state."); 
			return;
		}

        // Disable previous screen
        GameObject temp;
        if (screens.TryGetValue(newState, out temp))
        {
            temp.SetActive(true);
            if (current != screen.Invalid)
            {
                if (screens.TryGetValue(current, out temp))
                {
                    temp.SetActive(false);
                }
            }
            instance.current = newState;
        }
        else
        {
            print("No such state! State : " + newState );
        }
	}//end of function StateChange
		
}// end of UI_StateMachine
