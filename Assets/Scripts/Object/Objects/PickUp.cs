using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : ObjectBase
{
    public float radius;
    public override void InstantiateNewObject(GameObject platformObject, ObjectPanel objectPanel, out Platform platform)
    {
        base.InstantiateNewObject(platformObject, objectPanel, out platform);
        AddPickup(platformObject.transform.GetChild(0).gameObject);
    }

    public override void InstantiateOldObject(GameObject platformObject, int state, ObjectPanel panel, out Platform platform)
    {
        base.InstantiateOldObject(platformObject, state, panel, out platform);
        AddPickup(platformObject.transform.GetChild(0).gameObject);
    }

    public virtual void AddPickup(GameObject platformObject)
    {
        CircleCollider2D circleCollider = platformObject.AddComponent<CircleCollider2D>();
        circleCollider.radius = radius;
        circleCollider.isTrigger = true;
        if (LevelManager.Instance.isReplay) { platformObject.AddComponent<Rigidbody2D>().gravityScale = 0; }
    }
}