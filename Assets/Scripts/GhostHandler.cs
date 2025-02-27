using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHandler : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GhostPlayer ghost;

    static GhostHandler instance;
    public static GhostHandler Instance { get { return instance; } }

    public Action startGhost;

    bool started = false;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    private void Update()
    {
        if (started) { return; }
        started = true;
        if (LevelManager.Instance.isReplay) { BeginReplay(); }
        else { BeginGame(); }
    }

    private void BeginReplay()
    {
        Destroy(player);
        ghost.SetGhost(LevelManager.Instance.ghost);
        CameraMovement.Instance.cameraMode = CameraMode.FollowPlayer;
        CameraMovement.Instance.playerTransform = ghost.transform;
        Destroy(ReplaySaver.Instance.gameObject);
        startGhost();
    }

    private void BeginGame()
    {
        Destroy(ghost.gameObject);
        CameraMovement.Instance.cameraMode = CameraMode.FollowPoint;
        CameraMovement.Instance.playerTransform = player.transform;
        CenimaticCamera.Instance.Begin();
    }
}
