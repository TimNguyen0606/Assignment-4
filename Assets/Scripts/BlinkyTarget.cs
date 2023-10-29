using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkyTarget : MonoBehaviour
{
    [SerializeField]
    private Transform pacman;
    [SerializeField]
    private Ghost blinky;

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
        if (Input.GetKeyDown(KeyCode.B))
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

    private void UpdatePosition() // moves target position to pacman's position
    {
        Vector3 newPostion = pacman.position; // get pacman's position
        newPostion.z = transform.position.z; // make sure z value of target not affected
        transform.position = newPostion;

        UpdateLine(blinky.gameObject.transform.position, newPostion);
    }

    private void UpdateLine(Vector3 start, Vector3 end)
    {
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
