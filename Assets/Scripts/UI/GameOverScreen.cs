using UnityEngine.UI;	// for IMAGE class
using UnityEngine;


// You need to set the You Died and "score text" at inspector.
public class GameOverScreen : MonoBehaviour
{
    public static GameOverScreen instance;
    public Image youDied;           // You Died image to display when the player dies.
    public Text gameOverScoreText;  // Score text object. SET AT THE INSPECTOR!! it is under "game over ui" children. Font size should be "best fit" and "wrapped.

    //Awake is always called before any Start functions -Unity3D
    void Awake()
    {
        if (instance == null)                               // Check if instance already exists
            instance = this;                                // if not, set instance to "this"
        else if (instance != this)                          // If instance already exists and it's not this:
            Destroy(gameObject);                                // Then destroy this. Enforces the singleton pattern.
    }

    // Call this when player DIES
    public void GameOver()
    {
        gameOverScoreText.text = "SCORE: " + GameManager.instance.score.ToString();     // Show the Player Score at the screen.
        Invoke("OpenStartScreen", 4f);                                                  // If the player does nothing, close the game over screen automatically.
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Escape))
        {
            OpenStartScreen();
        }
    }

    void OpenStartScreen()
    {
        ScreenManager.instance.ChangeState(ScreenManager.screen.Start);
    }

}