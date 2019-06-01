using UnityEngine.UI;	// for IMAGE class
using UnityEngine;


// You need to set the You Died and "score text" at inspector.
public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    Text gameOverScoreText;  // Score text object. SET AT THE INSPECTOR!! it is under "game over ui" children. Font size should be "best fit" and "wrapped.

    void OnEnable()
    {
        gameOverScoreText.text = "SCORE: " + GameManager.instance.player.Score.ToString();     // Show the Player Score at the screen.
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