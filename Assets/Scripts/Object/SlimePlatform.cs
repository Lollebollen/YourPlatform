using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.transform.CompareTag("Player")) { return; }
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        player.speed = 0;
        player.rigid2D.gravityScale = 0;
        player.rigid2D.velocity = Vector3.zero;
        player.slimed = true;
        if (!player.slimes.Contains(this)) { player.slimes.Add(this); }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.transform.CompareTag("Player")) { return; }
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player.slimes.Contains(this)) { player.slimes.Remove(this); }
        if (player.slimes.Count > 0) { return; }
        player.speed = player.defaultSpeed;
        player.slimed = false;
    }
}
