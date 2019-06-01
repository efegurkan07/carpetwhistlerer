using System.Collections;
using UnityEngine;

// This script controls the player's interactions with the world and it's statistics
public class Player : MonoBehaviour 
{

    int score;                       // this is the player's score.
    public int Score {
        get {
            return score;
        }
    }   

    int remainingLives;						// current number of lives the player has.
    public int RemainingLives {
        get {
            return remainingLives;
        }
    }
	bool isInvincible = false; 						// set to true to prevent player from taking damage

    SpriteRenderer spriteRenderer;
    PlayerMovement movement;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<PlayerMovement>();
    }

    // Refresh Player
    public void RefreshPlayer()
	{
		remainingLives = GameManager.MaxLives; 	// Player begins with max lives. 
		isInvincible = false; 		// default = not invincible.
        movement.ResetPosition ();
        score = 0;
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
			} 
		}
	}// end of take damage

    void IncreaseLife()
    {
        remainingLives++;
    }

    void IncreasePoints()
    {
        score += GameManager.instance.pickupReward;			// Increase the player's score by 10
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
