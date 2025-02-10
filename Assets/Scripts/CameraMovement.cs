using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    PlayerMovement playerMovement;
    Transform playerTransform;
    MapBounds bounds;
    ObjectPanel panel;

    [SerializeField] float smooth;
    public CameraMode cameraMode = CameraMode.FollowPlayer;
    [HideInInspector] public Vector3 targerPoint;

    Vector2 cameraSize;
    Vector2 lastTouchPos;

    static CameraMovement instance;
    static public CameraMovement Instance {  get { return instance; } }

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
        cameraMode = CameraMode.FollowPoint;
    }

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        panel = FindObjectOfType<ObjectPanel>();
        bounds = FindObjectOfType<MapBounds>();
        playerTransform = playerMovement.transform;
        panel.StartedPlacingPlatforms += SetToStayStill;
        panel.PlacedOnePlatform += SetToDrag;
        panel.DonePlacingPlatforms += SetToFollow;
        cameraSize.y = Camera.main.orthographicSize;
        cameraSize.x = cameraSize.y * Camera.main.aspect;
    }

    private void LateUpdate()
    {
        switch (cameraMode)
        {
            case CameraMode.FollowPlayer:
                FollowPlayer();
                break;
            case CameraMode.Drag:
                Drag();
                break;
            case CameraMode.FollowPoint:
                Follow(targerPoint);
                break;
        }
    }

    private void FollowPlayer() { Follow(playerTransform.position + new Vector3(playerMovement.direction, 0, 0)); }

    private void Follow(Vector3 target)
    {
        Vector3 oldPos = transform.position;
        Vector3 newPos = Vector3.Lerp(oldPos, target, smooth);
        newPos = ClampToBounds(newPos);
        newPos.z = oldPos.z;
        transform.position = newPos;
    }

    private void Drag()
    {
        if (Input.touchCount < 1) { return; }
        Touch touch = Input.touches[0];
        if (touch.phase == TouchPhase.Began) { lastTouchPos = touch.position; }
        else if (touch.phase == TouchPhase.Moved)
        {
            Vector2 deltaTouch = Camera.main.ScreenToWorldPoint(touch.position) - Camera.main.ScreenToWorldPoint(lastTouchPos);
            Vector3 newPos = transform.position - (Vector3)deltaTouch;
            transform.position = ClampToBounds(newPos);
            lastTouchPos = touch.position;
        }
    }

    private Vector3 ClampToBounds(Vector3 newPos)
    {
        newPos.x = Mathf.Clamp(newPos.x, bounds.leftEdge + cameraSize.x, bounds.rightEdge - cameraSize.x);
        newPos.y = Mathf.Clamp(newPos.y, bounds.bottomEdge + cameraSize.y, bounds.topEdge + bounds.bottomEdge - cameraSize.y);
        return newPos;
    }

    public void SetToFollow()
    {
        cameraMode = CameraMode.FollowPlayer;
    }

    public void SetToDrag()
    {
        cameraMode = CameraMode.Drag;
    }

    public void SetToStayStill()
    {
        cameraMode = CameraMode.StayStill;
    }

    private void OnDisable()
    {
        panel.StartedPlacingPlatforms -= SetToStayStill;
        panel.PlacedOnePlatform -= SetToDrag;
        panel.DonePlacingPlatforms -= SetToFollow;
    }
}

public enum CameraMode : byte
{
    FollowPoint,
    FollowPlayer,
    Drag,
    StayStill
}
