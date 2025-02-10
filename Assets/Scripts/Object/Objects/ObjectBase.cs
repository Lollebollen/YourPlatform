using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[CreateAssetMenu(fileName = "new base platform object", menuName = "PlatformObject")]
public class ObjectBase : ScriptableObject
{
    static int num;
    public int ID;

    public Platform platformType;
    public Sprite image;
    public Vector2 placeSize;
    [HideInInspector] public BoxCollider2D placeCollider;

    public ObjectBase()
    {
        ID = ++num;
    }

    public virtual void InstantiateNewObject(GameObject platformObject, ObjectPanel objectPanel, out Platform platform)
    {
        // TODO Add different classes that inherit of platform here instead of always just platform
        var drag = platformObject.AddComponent<Drag>();
        placeCollider = platformObject.AddComponent<BoxCollider2D>();
        var rigid2D = platformObject.AddComponent<Rigidbody2D>();
        platform = CreateObject(platformObject, out SpriteRenderer spriteRenderer);
        drag.spriteRenderer = spriteRenderer;
        drag.colli2D = placeCollider;
        drag.rigid2D = rigid2D;
        drag.platform = platform;
        spriteRenderer.color = new Vector4(0, 1, 0, 0.5f);
        placeCollider.size = placeSize;
        rigid2D.gravityScale = 0;
        rigid2D.freezeRotation = true;
        platform.isNew = true;
        objectPanel.DonePlacingPlatforms += platform.OnPlaceComplete;
        platform.placingRigidBody2D = rigid2D;
        platform.placingCollider2D = placeCollider;
        platform.panel = objectPanel;
    }

    private Platform CreateObject(GameObject platformObject, out SpriteRenderer spriteRenderer)
    {
        spriteRenderer = platformObject.GetComponent<SpriteRenderer>();
        var platform = platformObject.AddComponent<Platform>();

        platform.ID = ID;
        spriteRenderer.sprite = image;

        return platform;
    }

    public virtual void InstantiateOldObject(GameObject platformObject, int state, ObjectPanel panel, out Platform platform)
    {
        platform = CreateObject(platformObject, out _);
        platform.state = state;
        platform.isNew = false;
    }
}
