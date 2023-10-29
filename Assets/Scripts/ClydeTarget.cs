using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClydeTarget : MonoBehaviour
{
    [SerializeField]
    private Transform pacman;
    [SerializeField]
    private Transform bottomLeftCorner;
    [SerializeField]
    private Ghost clyde;

    private bool visible = false;

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
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleVisiblity();
        }

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
        }
        else
        {
            sr.enabled = false;
            lr.enabled = false;
        }
    }

    private void UpdatePosition() // moves target position to pacman's position, if within 8 tiles then retreat to corner
    {
        float distance = Vector2.Distance(pacman.position, clyde.gameObject.transform.position); // get distance between pacman & Clyde

        Vector3 newPostion = new Vector3();

        if (distance > 8)
        {
            newPostion = pacman.position; // pacman is target
        }
        else
        {
            newPostion = bottomLeftCorner.position; // Clyde's corner is target
        }
        
        newPostion.z = transform.position.z; // make sure z value of target not affected
        transform.position = newPostion;

        UpdateLine(clyde.gameObject.transform.position, newPostion);
    }

    private void UpdateLine(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
