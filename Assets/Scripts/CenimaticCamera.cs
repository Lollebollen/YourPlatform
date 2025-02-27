using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenimaticCamera : MonoBehaviour
{
    CameraMovement cameraMovement;

    Vector3[] points;
    Vector3 lastPos;
    int currentPoint;

    [SerializeField] float deadZone;
    [SerializeField] float lerpAmount;

    static CenimaticCamera instance;
    public static CenimaticCamera Instance {  get { return instance; } }

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }

        int num = transform.childCount;
        points = new Vector3[num];
        for (int i = 0; i < num; i++)
        {
            points[i] = transform.GetChild(i).position;
            points[i].z = 0f;
        }
    }

    public void Begin()
    {
        cameraMovement = CameraMovement.Instance;
        cameraMovement.transform.position = new Vector3(points[0].x, points[0].y, cameraMovement.transform.position.z);
        cameraMovement.targerPoint = points[0];
    }

    private void Update() // TODO bezier curves intead? or just don't missuse lerp way dumbass
    {
        if (cameraMovement == null) { return; }
        Vector3 currentPos = cameraMovement.transform.position;
        currentPos.z = 0;
        if (deadZone * deadZone > (cameraMovement.targerPoint - points[currentPoint]).sqrMagnitude || currentPos == lastPos)
        {
            currentPoint++;
            if (currentPoint >= points.Length)
            {
                cameraMovement.cameraMode = CameraMode.StayStill;
                Destroy(this);
                SelectionPanel.Instance.Begin();
                return;
            }
        }
        cameraMovement.targerPoint = Vector3.Lerp(currentPos, points[currentPoint], lerpAmount);
        lastPos = new Vector3(cameraMovement.transform.position.x, cameraMovement.transform.position.y, 0);
    }
}