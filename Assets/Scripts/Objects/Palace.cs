using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Palace : FactoryObject
{
    protected override void RemoveObject()
    {
        PalaceFactory.instance.DestroyInstance(this);
    }
}
