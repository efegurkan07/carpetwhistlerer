using System.Collections;
using UnityEngine;

// This script controls the player's interactions with the world and it's statistics
public class Player : MonoBehaviour 
{
	// Player object
	public static Player instance;		// This is the player object. Place it in the inspector, ON THE CARPET RIDER. edit: game object idi (Player) playerscript yaptim. 

	// Variables
	public const int MaxLives = 2;					// max number of lives the player has.
	public int remainingLives;						// current number of lives the player has.
	public bool isInvincible; 						// set to true to prevent player from taking damage

    SpriteRenderer spriteRenderer;

	void Awake()									// Initialize the reference to this.
	{
		if (instance == null) 
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

    // Refresh Player
    public void RefreshPlayer()
	{
		this.remainingLives = MaxLives; 	// Player begins with max lives. 
		this.isInvincible = false; 		// default = not invincible.
		GameManager.instance.livesText.text = remainingLives.ToString ();	// Display remaining lives = max lives.
		PlayerMovement.instance.ResetPosition ();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }// end of refresh player


	/* Player took damage:
	 * - Decrease the number of lives.
	 * - Play the "hit" sound effect.
	 */
	void TakeDamage()
	{
		if (isInvincible)
			return;
		else 
		{            
			remainingLives--;
			if ( remainingLives >= 0) 								// If the player still has lives remaining;
			{
				StartCoroutine (PlayerDamaged ());					// Make the player invulnarable for a duration and blink.
                StartCoroutine(PlayerBlink());
				GameManager.instance.livesText.text = remainingLives.ToString ();	// Update the remaining lives at UI todo: bu variable'lari inGameUI'a tasi. 
			} 
			else 													// If the player has no more lives remaining; kill it.
			{
				GameManager.instance.GameOver (); 	// Start the game over routine.	
			} 
		}
	}// end of take damage

    void IncreaseLife ()
    {
        remainingLives++;                                   // PLUS HEALTH!
        GameManager.instance.livesText.text = Player.instance.remainingLives.ToString();	// update remaining lives = max lives.
    }

    void IncreasePoints()
    {
        GameManager.instance.GainScore(10);			// Increase the player's score by 10
    }


	// Blink the player and make it invincible for the duration 
	// Render the player invincible throughout the process.
	private readonly WaitForEndOfFrame wfeof = new WaitForEndOfFrame ();
    private readonly WaitForSeconds waitInvincible = new WaitForSeconds(3f);
    private readonly WaitForSeconds waitColor = new WaitForSeconds(0.3f);
    IEnumerator PlayerDamaged()
	{
		isInvincible = true; // Render player invincible while he is blinking
		yield return waitInvincible;
		isInvincible = false; // finish the invincibility period.
		yield return wfeof;
	}// end of player damaged

    IEnumerator PlayerBlink()
    {
        for (int i = 0; i < 10; i++)
        {
            spriteRenderer.color = (i % 2 == 0 ? Color.red : Color.white);
            yield return waitColor;
        }
        yield return wfeof;
    }

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "pickup") {
            IncreasePoints();
		} else if (col.gameObject.tag == "redpickup") {
            IncreaseLife();
		} 
		else if (col.gameObject.tag == "obstacle" && !isInvincible) 
		{
			TakeDamage (); 										// Take Damage
		}
	}//end of collider script

}// end of class PlayerScript
