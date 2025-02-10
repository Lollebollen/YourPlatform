using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new coin", menuName = "Coin")]
public class CoinScritableObject : PickUp
{
    public override void AddPickup(GameObject platformObject)
    {
        base.AddPickup(platformObject);
        platformObject.AddComponent<Coin>();
    }
}
