using UnityEngine;

public class FactoryObject : MonoBehaviour
{
    public new Rigidbody2D rigidbody;
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if ((rigidbody.position.x <= -10f) || (GameManager.instance.isPlaying == false))	// If the object has moved out of the screen, or if we quit the game.
        {
            RemoveObject();
        }
    }

    protected virtual void RemoveObject()
    {

    }

}
