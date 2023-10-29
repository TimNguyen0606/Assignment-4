using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkyTarget : MonoBehaviour
{
    [SerializeField]
    private Pacman pacman;
    [SerializeField]
    private Ghost pinky;

    private bool visible = false;
    public bool overflowBug = true;

    private SpriteRenderer sr;
    [SerializeField]
    private LineRenderer lr;

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
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleVisiblity();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            ToggleOverflowBug();
        }

        UpdatePosition();
    }

    private void ToggleVisiblity() // toggles visible boolean and runs a check
    {
        visible = !visible;

        CheckVisiblity();
    }

    private void ToggleOverflowBug() // toggles overflow bug, if true, then simulates the overflow bug in the original pacman where when pacman is facing up the target is also 4 units left of him
    {
        overflowBug = !overflowBug;
    }

    private void CheckVisiblity() // updates sprite visibility based on visible boolean
    {
        if (visible)
        {
            sr.enabled = true;
            lr.enabled = true;
        }
        else
        {
            sr.enabled = false;
            lr.enabled = false;
        }
    }

    private void UpdatePosition() // moves target position to 4 tiles ahead of pacman's position
    {
        Vector3 newPostion = pacman.gameObject.transform.position; // get pacman's position
        newPostion.z = transform.position.z; // make sure z value of target not affected

        if (pacman.movement.direction == Vector2.up)
        {
            newPostion.y += 4;

            if (overflowBug)
            {
                newPostion.x -= 4;
            }

            transform.position = newPostion;
        }
        else if (pacman.movement.direction == Vector2.down)
        {
            newPostion.y -= 4;
            transform.position = newPostion;
        }
        else if (pacman.movement.direction == Vector2.left)
        {
            newPostion.x -= 4;
            transform.position = newPostion;
        }
        else if (pacman.movement.direction == Vector2.right)
        {
            newPostion.x += 4;
            transform.position = newPostion;
        }

        UpdateLine(pinky.gameObject.transform.position, newPostion);
    }

    private void UpdateLine(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
