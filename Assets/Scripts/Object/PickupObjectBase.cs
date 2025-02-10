using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjectBase : MonoBehaviour
{
    public AudioClip clip;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) { PickupEffect(); }
    }

    public virtual void PickupEffect()
    {
        // TODO play audip clip
        Destroy(gameObject);
    }
}
