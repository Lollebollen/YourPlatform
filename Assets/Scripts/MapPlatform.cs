using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPlatform : MonoBehaviour
{
    Collider2D colli2D;
    Action replayStart;

    private void Awake()
    {
        var objectPanel = FindObjectOfType<ObjectPanel>();
        if (LevelManager.Instance.isReplay) { replayStart += OnPlaceComplete; }
        else { objectPanel.DonePlacingPlatforms += OnPlaceComplete; }
        colli2D = GetComponent<Collider2D>();
        colli2D.isTrigger = true;
    }

    private void Start()
    {
        replayStart?.Invoke();
    }

    public void OnPlaceComplete()
    {
        colli2D.isTrigger = false;
    }

    private void OnDisable()
    {
        replayStart -= OnPlaceComplete;
    }
}
