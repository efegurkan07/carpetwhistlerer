using UnityEngine;

// This script is put inside moving objects (obstacles or pick ups).
public class MovingObject : MonoBehaviour
{
    public float initialSpeed = 0.1f;
    public float acceleration = 0.1f;

    new Rigidbody2D rigidbody;
    float speed = 0.1f;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() 
    {
        speed = initialSpeed + (acceleration * ((Time.time - GameManager.instance.gameStartingTime) / 20f));
        rigidbody.position = new Vector3(rigidbody.position.x - speed, rigidbody.position.y, 0);
    }

}// end of ObjectMove script.
