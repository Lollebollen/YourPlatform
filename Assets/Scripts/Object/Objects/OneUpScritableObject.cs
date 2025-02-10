using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New OneUp", menuName = "1Up")]
public class OneUpScritableObject : PickUp
{
    public override void AddPickup(GameObject platformObject)
    {
        base.AddPickup(platformObject);
        platformObject.AddComponent<OneUp>();
    }
}
