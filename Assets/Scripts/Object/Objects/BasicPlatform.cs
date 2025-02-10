using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new basic platform object", menuName = "Basic Platform")]
public class BasicPlatform : ObjectBase
{
    public Vector2 platformSize;
    [HideInInspector] public BoxCollider2D boxCollider;

    public override void InstantiateNewObject(GameObject platformObject, ObjectPanel objectPanel, out Platform platform)
    {
        base.InstantiateNewObject(platformObject, objectPanel, out platform);

        AddCollider(platformObject, false);
    }

    private void AddCollider(GameObject platformObject, bool enableCollider)
    {
        boxCollider = platformObject.AddComponent<BoxCollider2D>();
        boxCollider.size = platformSize;
        boxCollider.enabled = enableCollider;
    }

    public override void InstantiateOldObject(GameObject platformObject, int state, ObjectPanel panel, out Platform platform)
    {
        base.InstantiateOldObject(platformObject, state, panel, out platform);
        AddCollider(platformObject, true);
        platformObject.AddComponent<MapPlatform>();
    }
}