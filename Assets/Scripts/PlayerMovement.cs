using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CircleCollider2D circleCollider;
    [HideInInspector] public Rigidbody2D rigid2D;

    public float defaultSpeed;
    [SerializeField] Vector2 jump = new(0, 10);
    [SerializeField] int jumpNumMax = 2;
    [SerializeField] float groundPound = 20;
    [SerializeField] float baseGravity = 1;
    [SerializeField] float gravityTweak = 2;
    [SerializeField] float wallGravityUp = 2;
    [SerializeField] float wallGravityDowm = 0.2f;
    [SerializeField] float wallAngleAlowence = 0.2f;
    [SerializeField] float dubbleTapTime = 0.1f;
    [SerializeField] float velocityClamp = 2;

    [HideInInspector] public float speed = 10;
    [HideInInspector] public int direction = 1;
    [HideInInspector] public bool slimed = false;
    bool wallGliding = false;
    bool groundPounding = false;
    bool grounded = false;
    int lastDirection = -1;
    int jumpNum;
    float lastInput;
    Vector3 spawnPosition;
    HashSet<Collider2D> walls = new();
    HashSet<Collider2D> grounds = new();
    public HashSet<SlimePlatform> slimes = new HashSet<SlimePlatform>();

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        var objectPanel = FindObjectOfType<ObjectPanel>();
        if (objectPanel != null)
        {
            enabled = false;
            rigid2D.gravityScale = 0;
            objectPanel.DonePlacingPlatforms += OnPlacingComplete;
        }
        spawnPosition = transform.position;
        speed = defaultSpeed;
    }

    private void Start()
    {
        FlagPole.Instance.LevelComplete += OnLevelComplete;
    }

    private void FixedUpdate()
    {
        Move();
        ChangeGravityScale(baseGravity, gravityTweak);
        if (wallGliding) { ClampVelocity(); }
    }

    private void Update()
    {
        HandleInput();
    }

    private void Move()
    {
        Vector3 velocity = rigid2D.velocity;
        rigid2D.velocity = new Vector3(speed * direction, velocity.y, velocity.z);
    }

    private void HandleInput()
    {
        if (Input.touchCount < 1) { return; }
        var phase = Input.GetTouch(0).phase;
        if (phase != TouchPhase.Began) { return; }

        if (Time.time - lastInput < dubbleTapTime) { GroundPund(); }
        else { Jump(); }
    }

    private void GroundPund()
    {
        lastInput = -1;
        if (grounded) { return; }
        rigid2D.velocity = new Vector2(0, groundPound);
        if (direction != 0) { lastDirection = direction; }
        direction = 0;
        groundPounding = true;
    }

    private void Jump()
    {
        lastInput = Time.time;
        if (direction == 0)
        {
            direction = -lastDirection;
            wallGliding = false;
        }
        else
        {
            if (jumpNum >= jumpNumMax) { return; }
            if (!grounded) { jumpNum++; }
        }
        groundPounding = false;

        Vector2 velocity = rigid2D.velocity;
        velocity.x += jump.x * direction;
        velocity.y = jump.y;
        rigid2D.velocity = velocity;
    }

    private void GravityTweak()
    {
        if (wallGliding)
        {
            ChangeGravityScale(wallGravityUp, wallGravityDowm);
        }
        else
        {
            ChangeGravityScale(1, gravityTweak);
        }
    }

    public void ClampVelocity()
    {
        Vector3 velocity = rigid2D.velocity;
        velocity.y = Mathf.Clamp(velocity.y, -velocityClamp, velocityClamp);
        rigid2D.velocity = velocity;
    }

    private void ChangeGravityScale(float up, float down)
    {
        if (slimed) { return; }
        if (grounded) { rigid2D.gravityScale = 1; }
        else if (rigid2D.velocity.y < 0) { rigid2D.gravityScale = down; }
        else { rigid2D.gravityScale = up; }
    }

    public void OnPlacingComplete()
    {
        this.enabled = true;
        rigid2D.gravityScale = 1;
    }

    public void OnLevelComplete()
    {
        // TODO explode
    }

    public void ChangeDirection()
    {
        direction = -direction;
    }

    public void ResetPlayer()
    {
        direction = 1;
        lastDirection = -1;
        wallGliding = false;
        groundPounding = false;
        rigid2D.velocity = Vector3.zero;
        transform.position = spawnPosition;
        grounds = new();
        walls = new();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 pos = transform.position;
        List<ContactPoint2D> contactPoints = new();
        collision.GetContacts(contactPoints);

        foreach (ContactPoint2D point in contactPoints)
        {
            float dot = Vector3.Dot(new(direction, 0, 0), (Vector3)point.normal);
            if (dot > wallAngleAlowence)
            {
                if (groundPounding) { direction = lastDirection; groundPounding = false; }
                if (point.point.y > pos.y) { continue; }
                grounded = true;
                if (grounded) { jumpNum = 0; }
                if (!grounds.Contains(collision.collider)) { grounds.Add(collision.collider); }
                continue;
            }
            if (dot > 0) { continue; }
            else
            {
                if (grounded) { (lastDirection, direction) = (direction, lastDirection); }
                else
                {
                    if (direction != 0) { lastDirection = direction; }
                    direction = 0;
                    wallGliding = true;
                    if (!walls.Contains(collision.collider)) { walls.Add(collision.collider); }
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision == null) { return; }
        if (grounds.Contains(collision.collider)) { grounds.Remove(collision.collider); }
        if (walls.Contains(collision.collider)) { walls.Remove(collision.collider); }
        grounded = grounds.Count != 0;

        wallGliding = false;
        if (direction == 0) { direction = lastDirection; }
    }
}
