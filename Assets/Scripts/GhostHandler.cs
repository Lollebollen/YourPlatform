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
        CameraMovement.Instance.cameraMode = CameraMode.FollowPlayer;
        CameraMovement.Instance.playerTransform = ghost.transform;
        Destroy(ReplaySaver.Instance.gameObject);
    }

    private void BeginGame()
    {
        Destroy(ghost.gameObject);
        CameraMovement.Instance.cameraMode = CameraMode.FollowPoint;
        CameraMovement.Instance.playerTransform = player.transform;
        CenimaticCamera.Instance.Begin();
    }
}
