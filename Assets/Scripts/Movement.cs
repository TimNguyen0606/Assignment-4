using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public Rigidbody2D rigidBody { get; private set; }

    [SerializeField]
    private float speed = 8.0f;
    public float speedMultiplier = 1.0f;

    [SerializeField]
    private Vector2 intialDirection;
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    public Vector3 startingPostion { get; private set; }

    [SerializeField]
    private LayerMask obstacleLayer;

    [SerializeField]
    private bool resetMultiplierAtStart = true;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        startingPostion = transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState() // reset variables to default values
    {
        if (resetMultiplierAtStart)
        {
            speedMultiplier = 1.0f;
            direction = intialDirection;
            nextDirection = Vector2.zero;
            transform.position = startingPostion;
        }
        
        //direction = intialDirection;
        //nextDirection = Vector2.zero;
        //transform.position = startingPostion;
        rigidBody.isKinematic = false;
        this.enabled = true;
    }

    private void Update()
    {
        if (nextDirection != Vector2.zero) // if we have a nextDirection, call setDirection to see if it can become current direction
        {
            SetDirection(nextDirection);
        }
    }

    private void FixedUpdate() // where most of physics is done, frame independent
    {
        Vector2 position = rigidBody.position;
        Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;

        rigidBody.MovePosition(position + translation);
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if (forced || !Occupied(direction)) // if space not occupied set current direction to direction, otherwise queue the direction for when available
        {
            this.direction = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }
    }

    public bool Occupied(Vector2 direction) // checks to see if path is blocked using a raycast
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0.0f, direction, 1.5f, obstacleLayer); // pos, size of box, angle, direction, distance, layer to hit
        return hit.collider != null;
    }
}
