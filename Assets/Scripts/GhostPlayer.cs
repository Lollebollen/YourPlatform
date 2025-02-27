using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlayer : MonoBehaviour
{
    Ghost ghost;
    float startTime;
    int i = 1;

    public void SetGhost(Ghost ghost)
    {
        this.ghost = ghost;
        startTime = Time.time;
    }

    private void Update()
    {
        if (ghost == null) { return; }
        FollowGhost();
    }

    private void FollowGhost()
    {
        float ratio;
        do
        {
            if (Time.time - startTime > ghost.times[ghost.times.Length - 1]) { Finished(); return; }
            ratio = (Time.time - startTime - ghost.times[i - 1]) / (ghost.times[i] - ghost.times[i - 1]);
            if (ratio < 1) { break; }
            else { i++; }
        } while (true);

        Vector3 newPos = new Vector3(ratio * (ghost.x[i] - ghost.x[i - 1]) + ghost.x[i - 1], ratio * (ghost.y[i] - ghost.y[i - 1]) + ghost.y[i - 1], transform.position.z);
        transform.position = newPos;
    }

    private void Finished()
    {
        LevelManager.Instance.OnReplayComplete();
    }
}
