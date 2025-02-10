using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Collider2D placingCollider2D;
    public Rigidbody2D placingRigidBody2D;
    public ObjectPanel panel;

    public int ID;
    public int state;
    public bool isNew;

    public void OnPlaceComplete()
    {
        Destroy(placingCollider2D);
        Destroy(placingRigidBody2D);

        foreach (var item in gameObject.GetComponentsInChildren<Collider2D>())
        {
            item.enabled = true;
        }
    }

    private void Start()
    {
        HandleState();
    }

    public virtual Quaternion HandleState()
    {
        Quaternion quaternion = Quaternion.identity;
        switch (state % 4)
        {
            case 0:
                quaternion = Quaternion.Euler(new Vector3(0, 0, 0));
                transform.rotation = quaternion;
                break;
            case 1:
                quaternion = Quaternion.Euler(new Vector3(0, 0, 90));
                transform.rotation = quaternion;
                break;
            case 2:
                quaternion = Quaternion.Euler(new Vector3(0, 0, 180));
                transform.rotation = quaternion;
                break;
            case 3:
                quaternion = Quaternion.Euler(new Vector3(0, 0, 270));
                transform.rotation = quaternion;
                break;
        }
        return quaternion;
    }

    private void OnDisable()
    {
        if (panel == null) { return; }
        panel.DonePlacingPlatforms -= OnPlaceComplete;
    }
}
