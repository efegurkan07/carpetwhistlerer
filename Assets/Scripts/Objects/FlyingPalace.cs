using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPalace : FactoryObject
{
    protected override void RemoveObject()
    {
        FlyingPalaceFactory.instance.DestroyInstance(this);
    }
}
