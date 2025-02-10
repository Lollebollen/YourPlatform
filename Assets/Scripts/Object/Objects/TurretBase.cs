using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Turret", menuName = "Turret")]
public class TurretBase : ObjectBase
{
    public GameObject bullet;
    public Vector2 shootOffset;
    public float shootDelay;
    public float shootSpeed;

    public override void InstantiateNewObject(GameObject platformObject, ObjectPanel objectPanel, out Platform platform)
    {
        base.InstantiateNewObject(platformObject, objectPanel, out platform);
        AddTurret(platformObject, objectPanel);
    }

    public override void InstantiateOldObject(GameObject platformObject, int state, ObjectPanel panel, out Platform platform)
    {
        base.InstantiateOldObject(platformObject, state, panel, out platform);
        AddTurret(platformObject, panel);
    }

    public void AddTurret(GameObject platformObject, ObjectPanel panel)
    {
        Turret turret = platformObject.AddComponent<Turret>();
        turret.bullet = bullet;
        turret.shootOffset = shootOffset;
        turret.shootDelay = shootDelay;
        turret.shootSpeed = shootSpeed;
        if (panel != null) { panel.DonePlacingPlatforms += turret.BeginShoot; }
    }
}
