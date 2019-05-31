using UnityEngine;

public class Pickup : FactoryObject
{
    protected override void RemoveObject()
    {
        PickupFactory.instance.DestroyInstance(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            RemoveObject();
        }
    }
}
