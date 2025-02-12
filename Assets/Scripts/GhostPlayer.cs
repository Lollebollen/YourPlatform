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
        float ratio = (Time.time - startTime - ghost.times[i - 1]) / (ghost.times[i] - ghost.times[i - 1]);

        if (ratio > 1) 
        { 
            i++; 
            if (i > ghost.times.Length) { return; }// TODO end replay
            FollowGhost(); 
            return; 
        }

        Vector3 newPos = new Vector3(ratio * (ghost.x[i] - ghost.x[i - 1]) + ghost.x[i - 1], ratio * (ghost.y[i] - ghost.y[i - 1]) + ghost.y[i - 1], transform.position.z);
        transform.position = newPos;
    }
}
