using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new slime platform object", menuName = "slime Platform")]
public class SlimePlatformObject : BasicPlatform
{
    public override void InstantiateNewObject(GameObject platformObject, ObjectPanel objectPanel, out Platform platform)
    {
        base.InstantiateNewObject(platformObject, objectPanel, out platform);

        platformObject.AddComponent<SlimePlatform>();
    }

    public override void InstantiateOldObject(GameObject platformObject, int state, ObjectPanel panel, out Platform platform)
    {
        base.InstantiateOldObject(platformObject, state, panel, out platform);

        platformObject.AddComponent<SlimePlatform>();
    }
}
