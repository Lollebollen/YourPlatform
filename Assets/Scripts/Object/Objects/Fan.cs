using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "new fan object", menuName = "fan")]
public class Fan : BasicPlatform
{
    public AnimatorController controller;
    public float colliderOffset = -0.186f;
    public GameObject fanArea;

    public override void InstantiateNewObject(GameObject platformObject, ObjectPanel objectPanel, out Platform platform)
    {
        base.InstantiateNewObject(platformObject, objectPanel, out platform);
        placeCollider.offset = new Vector2(0, colliderOffset);

        AddFan(platformObject);
    }

    private void AddFan(GameObject platformObject)
    {

        platformObject.AddComponent<Animator>().runtimeAnimatorController = controller;
        boxCollider.offset = new Vector2(0, colliderOffset);
        Instantiate(fanArea, platformObject.transform.position, Quaternion.identity, platformObject.transform);
    }

    public override void InstantiateOldObject(GameObject platformObject, int state, ObjectPanel panel, out Platform platform)
    {
        base.InstantiateOldObject(platformObject, state, panel, out platform);

        AddFan(platformObject);
    }
}
