using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEyes : MonoBehaviour
{
    private SpriteRenderer sr;
    private Movement movement;

    [SerializeField]
    private Sprite up;
    [SerializeField]
    private Sprite down;
    [SerializeField]
    private Sprite left;
    [SerializeField]
    private Sprite right;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<Movement>();
    }

    private void Update()
    {
        if (movement.direction == Vector2.up)
        {
            sr.sprite = up;
        }
        else if (movement.direction == Vector2.down)
        {
            sr.sprite = down;
        }
        else if (movement.direction == Vector2.left)
        {
            sr.sprite = left;
        }
        else if (movement.direction == Vector2.right)
        {
            sr.sprite = right;
        }
    }
}
