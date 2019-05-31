public class PickupFactory : Factory
{
    public static PickupFactory instance;

    protected override void Start()
    {
        base.Start();
        if (instance == null)                               // Check if instance already exists
            instance = this;                                // if not, set instance to "this"
        else if (instance != this)                          // If instance already exists and it's not this:
            Destroy(gameObject);                            // Then destroy this. Enforces the singleton pattern.
    }
}
