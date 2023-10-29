using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkyTarget : MonoBehaviour
{
    [SerializeField]
    private Pacman pacman;
    [SerializeField]
    private Ghost blinky;
    [SerializeField]
    private Ghost inky;
    [SerializeField]
    private PinkyTarget pinkyTarget;

    [SerializeField]
    private GameObject pacFront;

    private bool visible = false;
    private bool overflowBug = true;

    private SpriteRenderer sr;

    [SerializeField]
    private LineRenderer lr;
    [SerializeField]
    private LineRenderer inkyLr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        CheckVisiblity();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleVisiblity();
        }

        overflowBug = pinkyTarget.overflowBug;

        UpdatePosition();
    }

    private void ToggleVisiblity() // toggles visible boolean and runs a check
    {
        visible = !visible;

        CheckVisiblity();
    }

    private void CheckVisiblity() // updates sprite visibility based on visible boolean
    {
        if (visible)
        {
            sr.enabled = true;
            lr.enabled = true;
            inkyLr.enabled = true;
            pacFront.SetActive(true);
        }
        else
        {
            sr.enabled = false;
            lr.enabled = false;
            inkyLr.enabled = false;
            pacFront.SetActive(false);
        }
    }

    private void UpdatePosition() // moves to target position which is calculated using pos 2 tiles in front of pacman & blinky's location
    {
        Vector3 pacmanFront = pacman.gameObject.transform.position; // get pacman's position
        pacmanFront.z = transform.position.z; // make sure z value of target not affected

        if (pacman.movement.direction == Vector2.up)
        {
            pacmanFront.y += 2;

            if (overflowBug)
            {
                pacmanFront.x -= 2;
            }
        }
        else if (pacman.movement.direction == Vector2.down)
        {
            pacmanFront.y -= 2;
        }
        else if (pacman.movement.direction == Vector2.left)
        {
            pacmanFront.x -= 2;
        }
        else if (pacman.movement.direction == Vector2.right)
        {
            pacmanFront.x += 2;
        }

        pacFront.transform.position = pacmanFront;

        Vector3 blinkyPos = blinky.gameObject.transform.position;
        blinkyPos.z = transform.position.z; // make sure z value of target not affected

        Vector3 newPos = new Vector3();

        newPos.x = 2 * pacmanFront.x - blinkyPos.x;
        newPos.y = 2 * pacmanFront.y - blinkyPos.y;
        newPos.z = transform.position.z;

        transform.position = newPos;

        UpdateLine(blinkyPos, newPos);
        UpdateInkyLine(inky.gameObject.transform.position, newPos);
    }

    private void UpdateLine(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    private void UpdateInkyLine(Vector3 start, Vector3 end)
    {
        inkyLr.SetPosition(0, start);
        inkyLr.SetPosition(1, end);
    }
}
