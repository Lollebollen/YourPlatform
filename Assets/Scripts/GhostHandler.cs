using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHandler : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GhostPlayer ghost;

    private void Start()
    {
        if (LevelManager.Instance.isReplay) { BeginReplay(); }
        else { BeginGame(); }
    }

    private void BeginReplay()
    {
        Destroy(player);
        ghost.SetGhost(LevelManager.Instance.ghost);
    }

    private void BeginGame()
    {
        Destroy(ghost.gameObject);
        CenimaticCamera.Instance.Begin();
    }
}
