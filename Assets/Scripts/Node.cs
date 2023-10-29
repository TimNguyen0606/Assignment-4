using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField]
    private LayerMask obstacleLayer;

    public List<Vector2> availableDirections { get; private set; }

    private void Start()
    {
        availableDirections = new List<Vector2>();

        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }

    private void CheckAvailableDirection(Vector2 direction) // checks to see if wall in way of direction. If not, then add direction to list of possible directions
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0.0f, direction, 1.0f, obstacleLayer); // pos, size of box, angle, direction, distance, layer to hit

        if (hit.collider == null)
        {
            availableDirections.Add(direction);
        }
    }
}
