using UnityEngine;

public class PalaceFactory : Factory
{
    public static PalaceFactory instance;

    protected override void Start()
    {
        base.Start();
        if (instance == null)                               // Check if instance already exists
            instance = this;                                // if not, set instance to "this"
        else if (instance != this)                          // If instance already exists and it's not this:
            Destroy(gameObject);                            // Then destroy this. Enforces the singleton pattern.
    }

    public override FactoryObject CreateInstance(float height)
    {
        FactoryObject temp = base.CreateInstance(height);
        if (temp == null)
            return null;

        if (height < 0)
        {
            temp.rigidbody.rotation = 180f;
            temp.rigidbody.position = new Vector2(temp.rigidbody.position.x, 6.8f);
        }
        else
        {
            temp.rigidbody.rotation = 0f;
            temp.rigidbody.position = new Vector2(temp.rigidbody.position.x, -6.8f);
        }
        return temp;
    }
}
