using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlatform : MonoBehaviour
{
    Collider2D colli2D;

    private void Awake()
    {
        var objectPanel = FindObjectOfType<ObjectPanel>();
        objectPanel.DonePlacingPlatforms += OnPlaceComplete;
        colli2D = GetComponent<Collider2D>();
        colli2D.isTrigger = true;
    }

    public void OnPlaceComplete()
    {
        colli2D.isTrigger = false;
    }
}
