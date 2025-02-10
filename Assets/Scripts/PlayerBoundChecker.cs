using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoundChecker : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerLifes playerLifes;

    Vector2 playerSize;
    Vector3 mapBounds;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerLifes = PlayerLifes.Instance;
        playerSize = GetComponent<Collider2D>().bounds.extents;
        var mapBounds = FindObjectOfType<MapBounds>();
        if (mapBounds == null) { return; }
        this.mapBounds = new Vector3(mapBounds.leftEdge, mapBounds.rightEdge, mapBounds.bottomEdge);
    }

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;
        if (pos.x - playerSize.x < mapBounds.x) { playerMovement.ChangeDirection(); }
        if (pos.x + playerSize.x > mapBounds.y) { playerMovement.ChangeDirection(); }
        if (pos.y - playerSize.y < mapBounds.z) { playerLifes.LoseLife(); }
    }
}