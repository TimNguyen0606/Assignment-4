using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEyes : MonoBehaviour
{
    private SpriteRenderer sr;   // Reference to the SpriteRenderer component
    private Movement movement;   // Reference to the Movement component

    [SerializeField]
    private Sprite up;   // Sprite for the "up" direction
    [SerializeField]
    private Sprite down; // Sprite for the "down" direction
    [SerializeField]
    private Sprite left; // Sprite for the "left" direction
    [SerializeField]
    private Sprite right; // Sprite for the "right" direction

    private void Awake()
    {
        // Get the SpriteRenderer component attached to this object
        sr = GetComponent<SpriteRenderer();

        // Get the Movement component from the parent object
        movement = GetComponentInParent<Movement>();
    }

    private void Update()
    {
        // Update the sprite displayed on the GhostEyes based on the current movement direction
        if (movement.direction == Vector2.up)
        {
            sr.sprite = up; // Set the sprite to the "up" sprite
        }
        else if (movement.direction == Vector2.down)
        {
            sr.sprite = down; // Set the sprite to the "down" sprite
        }
        else if (movement.direction == Vector2.left)
        {
            sr.sprite = left; // Set the sprite to the "left" sprite
        }
        else if (movement.direction == Vector2.right)
        {
            sr.sprite = right; // Set the sprite to the "right" sprite
        }
    }
}
