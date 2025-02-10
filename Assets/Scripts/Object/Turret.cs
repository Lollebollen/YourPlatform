using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bullet;
    public Vector2 shootOffset;
    public float shootDelay;
    public float shootSpeed;

    public void BeginShoot()
    {
        StartCoroutine(ShootCycle());
    }

    IEnumerator ShootCycle()
    {
        yield return new WaitForSeconds(shootDelay);
        Shoot();
        StartCoroutine(ShootCycle());
    }

    private void Shoot()
    {
        // TODO ObjetPool
        Vector2 modifiedOffset = transform.up * shootOffset.y + transform.right * shootOffset.x;
        GameObject bulletObject = Instantiate(bullet, transform.position + (Vector3)modifiedOffset, Quaternion.identity, transform);
        bulletObject.transform.rotation = transform.rotation;
        bulletObject.GetComponent<Rigidbody2D>().velocity = -transform.right * shootSpeed;
    }
}
