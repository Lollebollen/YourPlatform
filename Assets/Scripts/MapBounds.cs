using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBounds : MonoBehaviour
{
    public float leftEdge;
    public float rightEdge;
    public float bottomEdge;
    public float topEdge;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(new Vector3(leftEdge, bottomEdge, 0), new Vector3(0, topEdge, 0));
        Gizmos.DrawRay(new Vector3(rightEdge, bottomEdge, 0), new Vector3(0, topEdge, 0));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(leftEdge, bottomEdge, 0), new Vector3(rightEdge, bottomEdge, 0));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(leftEdge, topEdge + bottomEdge, 0), new Vector3(rightEdge, topEdge + bottomEdge, 0));
    }
}
