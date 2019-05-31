using UnityEngine;

public class Tower : FactoryObject
{
    protected override void RemoveObject()
    {
        TowerFactory.instance.DestroyInstance(this);
    }
}
