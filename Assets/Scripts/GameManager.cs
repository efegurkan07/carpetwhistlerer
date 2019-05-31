﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;  
    
    // Coroutine Varianbles
	public WaitForEndOfFrame wfeof = new WaitForEndOfFrame ();	// wfeof is used in obstacles and other stuff.

    IEnumerator pickupSpawnCoroutine;
    IEnumerator healthSpawnCoroutine;
    IEnumerator obstacleSpawnCoroutine;

    // Game Variables
	public float gameStartingTime = 0f;
	public bool isPlaying;											    // True during game/ingame state. false otherwise.
	public static int HighScore = 0; 							        // Updated by PlayerPrefs "HighScore".
	public int score;                                                   // this is the player's score.

    public float minimumPickupSpawnRate = 5f;
    public float maximumPickupSpawnRate = 15f;
    public float minimumHealthSpawnRate = 30f;
    public float maximumHealthSpawnRate = 60f;
    public float minimumWaitAfterSimpleObstacle = 2f;
    public float maximumWaitAfterSimpleObstacle = 4f;
    public float minimumWaitAfterComplexObstacle = 3f;
    public float maximumWaitAfterComplexObstacle = 6f;

    // Some UI Labels 
    public Text scoreText;                     // The score text at the UI
    public Text livesText;                     // UI text that keeps track of the remaining lives.			
    public Text highscoreText;                 // This remains on the screen all the time.

    // Misc
    public AudioClip bgMusic;                   // Background music todo fix name and music
    public AudioClip hit; 						// When the player hits an obstacle, this will play.
    public GameObject noteAnimation;            // Nota animasyonu todo fix name
    public GameObject cube;                     // halinin en ondeki objesi sanirim		


    //Awake is always called before any Start functions -Unity3D
    void Awake()
	{
		if (instance == null) 								// Check if instance already exists
			instance = this; 								// if not, set instance to "this"
		else if (instance != this)							// If instance already exists and it's not this:
			Destroy(gameObject);    						// Then destroy this. Enforces the singleton pattern.
		DontDestroyOnLoad(gameObject);                      //Sets this to not be destroyed when reloading scene
	}
		
	void Start()
	{
		PlayerPrefs.SetInt ("HighScore", 3);					// TODO: delete before demo.
		HighScore = PlayerPrefs.GetInt ("HighScore");			// Pull the saved value from PlayerPrefs. If there is no value, it returns 0.
		ResetGame(); // Initialize the game

        // Set Coroutines
        pickupSpawnCoroutine = SpawnPickups();
        healthSpawnCoroutine = SpawnHealth();
        obstacleSpawnCoroutine = SpawnObstacles();
    }

	// Called at start. & end.
	public static void ResetGame ()
	{
		instance.isPlaying = false;											// Not in game. in start or something probably.
		instance.score = 0;											// Set the score to 0 at the beginning of the game.
		instance.scoreText.text = "0";										// When the player score changes, update the string at UI too.
		instance.highscoreText.text = "highscore: " + HighScore.ToString();	// Updated only when the high score changes.
		Player.instance.RefreshPlayer ();                           // Maximise the health etc.
        PlayerMovement.instance.ResetPosition();                                                    // Resets the player position and movement, at the left of screen. 
    }
		
	// Called at mainmenuUI to start the game
    public void StartGame()
    {
		// bring inGameUI at inspector.
		ResetGame ();
		isPlaying = true;  														// For obstacle_manager objects. pause doesnt make it false.
        gameStartingTime = Time.time;                                           // For obstacle creation, speed, etc.

        StartFactories();
    }
		
	// This function is called by the PlayerScript when the player loses all lives.
	public void GameOver()
	{
		EndGame ();
		ScreenManager.instance.ChangeState (ScreenManager.screen.GameOver);	// bring gameoverUI.
		GameOverScreen.instance.GameOver ();
	}// end of game over.


	public void EndGame()
	{
		isPlaying = false;                                      // make the obstacles & pick ups disappear.
        PlayerMovement.instance.ResetPosition();                // Resets the player position and movement, at the left of screen. 

        StopFactories();
    }

	// Pick-Ups call this function when the player gains points from pick-ups.
	public void GainScore( int points)
	{
		instance.score += points;
		instance.scoreText.text = instance.score.ToString();	// When the player score changes, update the string at UI too.
		if (HighScore < instance.score) 
		{
			UpdateHighScore ();
		}
	}

	private void UpdateHighScore()
	{
        HighScore = instance.score;                                                     // Update the highscore attribute.
        PlayerPrefs.SetInt("HighScore", HighScore);                                     // saves the highscore between sessions.
        instance.highscoreText.text = "highscore: " + HighScore.ToString();             // Update the HighScore text on screen. 
    }

    void StartFactories()
    {
        StartCoroutine(pickupSpawnCoroutine);
        StartCoroutine(healthSpawnCoroutine);
        StartCoroutine(obstacleSpawnCoroutine);
    }

    void StopFactories()
    {
        StopCoroutine(pickupSpawnCoroutine);
        StopCoroutine(healthSpawnCoroutine);
        StopCoroutine(obstacleSpawnCoroutine);
    }

#region Coroutines for factories

    IEnumerator SpawnPickups()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minimumPickupSpawnRate, maximumPickupSpawnRate));
            PickupFactory.instance.CreateInstance(Random.Range(-12f, 12f));
        }
        
    }

    IEnumerator SpawnHealth()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(minimumHealthSpawnRate, maximumHealthSpawnRate));
            HealthFactory.instance.CreateInstance(Random.Range(-12f, 12f));
        }
    }

    IEnumerator SpawnObstacles()
    {
        while(true)
        {
            float value = Random.value;
            if(value < 0.1f)
            {
                PalaceFactory.instance.CreateInstance(Random.value - 0.5f);
                yield return new WaitForSeconds(Random.Range(minimumWaitAfterComplexObstacle, maximumWaitAfterComplexObstacle));
            } else if (value < 0.25f)
            {
                TowerFactory.instance.CreateInstance(0f);
                TowerFactory.instance.CreateInstance(-1f); //flipped
                yield return new WaitForSeconds(Random.Range(minimumWaitAfterComplexObstacle, maximumWaitAfterComplexObstacle));
            } else if(value < 0.4f)
            {
                FlyingPalaceFactory.instance.CreateInstance(0f);
                yield return new WaitForSeconds(Random.Range(minimumWaitAfterComplexObstacle, maximumWaitAfterComplexObstacle));
            } else 
            {
                TowerFactory.instance.CreateInstance(Random.value - 0.5f);
                yield return new WaitForSeconds(Random.Range(minimumWaitAfterSimpleObstacle, maximumWaitAfterSimpleObstacle));
            }
        }
    }

#endregion

}// end of game manager.