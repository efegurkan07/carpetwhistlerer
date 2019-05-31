using UnityEngine;

public class InGameScreen : MonoBehaviour {

	public static InGameScreen instance;

	void Awake()
	{
		// Enforce singleton
		if (instance == null) 
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}// end of awake function


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) )
        {
            if ( Time.timeScale == 0)
                Time.timeScale = 1;
            else
                Time.timeScale = 0;
        }
    }// end of Update function
}// end of class ingame ui .
