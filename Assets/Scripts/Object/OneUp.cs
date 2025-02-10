using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneUp : PickupObjectBase
{
    public override void PickupEffect()
    {
        PlayerLifes.Instance.GainLife();
        base.PickupEffect();
    }
}
