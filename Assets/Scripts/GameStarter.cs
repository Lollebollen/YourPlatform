using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    private void Awake()
    {
        LevelManager.Instance.StartGame();
    }
}
