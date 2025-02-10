using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Drag : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Collider2D colli2D;
    public Rigidbody2D rigid2D;
    public Platform platform;

    Camera cam;

    float minDistance = 0.05f;

    Vector2 lastPos;
    Vector2 firstPos;
    Dictionary<Collider2D, Collider2D> colliders2Ds = new();

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        DragObject();
    }

    private void DragObject()
    {
        if (Input.touches.Length < 1) { return; }
        Touch touch = Input.touches[0];
        if (touch.phase == TouchPhase.Began) { lastPos = touch.position; firstPos = lastPos; }
        else if (touch.phase == TouchPhase.Moved && (touch.position - firstPos).sqrMagnitude > Mathf.Pow(minDistance, 2))
        {
            Vector3 nudge = cam.ScreenToWorldPoint(touch.position) - cam.ScreenToWorldPoint(lastPos);
            nudge.z = 0;
            transform.position += nudge;
            lastPos = touch.position;
        }
        else if (touch.phase == TouchPhase.Ended && (touch.position - firstPos).sqrMagnitude < Mathf.Pow(minDistance, 2))
        {
            platform.state++;
            platform.HandleState();
        }
    }

    public bool CanPlace()
    {
        return colliders2Ds.Count < 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && !colliders2Ds.ContainsKey(collision)) { colliders2Ds.Add(collision, collision); }
        spriteRenderer.color = new Vector4(1, 0, 0, 0.5f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && colliders2Ds.ContainsKey(collision)) { colliders2Ds.Remove(collision); }
        if (CanPlace()) { spriteRenderer.color = new Vector4(0, 1, 0, 0.5f); }
    }

    private void OnDisable()
    {
        colli2D.isTrigger = true;
        Destroy(rigid2D);
        spriteRenderer.color = Color.white;
    }
}