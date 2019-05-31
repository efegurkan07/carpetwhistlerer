using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : FactoryObject
{
    protected override void RemoveObject()
    {
        HealthFactory.instance.DestroyInstance(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            RemoveObject();
        }
    }
}
