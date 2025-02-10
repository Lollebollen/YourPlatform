using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlagPole : MonoBehaviour
{
    static FlagPole instance;
    public static FlagPole Instance {  get { return instance; } }

    public Action LevelComplete;

    private void Awake()
    {
        if (Instance == null) { instance = this; }
        else { Destroy(this); }
    }

    private void Start()
    {
        LevelComplete += LevelManager.Instance.OnLevelComplete;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) {  return; }
        LevelComplete?.Invoke();
    }
}
