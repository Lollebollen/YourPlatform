using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    MapBounds mapBounds;

    private void Awake()
    {
        mapBounds = FindObjectOfType<MapBounds>();
    }

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;
        if (mapBounds.rightEdge < pos.x) { DestroySelf(); }
        else if (mapBounds.leftEdge > pos.x) { DestroySelf(); }
        else if (mapBounds.bottomEdge > pos.y) { DestroySelf(); }
        else if (mapBounds.bottomEdge + mapBounds.topEdge < pos.y) { DestroySelf(); }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player")) { PlayerLifes.Instance.LoseLife(); }
        DestroySelf();
    }

    private void DestroySelf()
    {
        // TODO add effects
        Destroy(gameObject);
    }
}
