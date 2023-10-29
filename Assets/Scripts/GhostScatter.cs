using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScatter : GhostBehavior // picks a random direction at each node (intersection), tries to avoid backtracking if possible
{
    [SerializeField]
    private Transform targetCorner;
    [SerializeField]
    private GameManager gm;

    //private void Awake()
    //{
    //    gm = FindObjectOfType<GameManager>();
    //}

    private void OnDisable() // transistion to chase state
    {
        //Debug.Log("Chase: " + gm.CalculateChaseTime());
        ghost.chase.Enable(gm.CalculateChaseTime());
        Vector2 priorDirection = ghost.movement.direction;
        ghost.movement.SetDirection(-priorDirection); // make ghost turn around
    }

    private void OnTriggerEnter2D(Collider2D collision) // Ghosts will head to their respective corners
    {
        Node node = collision.GetComponent<Node>();

        if (node != null && this.enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                float distance = (targetCorner.position - newPosition).sqrMagnitude; // calculate distance from possible direction to target being chased. sqrMagnitude better for performance than magnitude

                if (availableDirection == -ghost.movement.direction && node.availableDirections.Count > 1) // prevents ghost from backtracking
                {

                }
                else
                {
                    if (distance < minDistance) // if this path is shorter, set direction to this direction
                    {
                        direction = availableDirection;
                        minDistance = distance;
                    }
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Node node = collision.GetComponent<Node>();

    //    if (node != null && this.enabled && !ghost.frightened.enabled)
    //    {
    //        int index = Random.Range(0, node.availableDirections.Count);

    //        if (node.availableDirections[index] == -ghost.movement.direction && node.availableDirections.Count > 1) // making sure ghost doesn't backtrack and look stupid
    //        {
    //            index++;

    //            if (index >= node.availableDirections.Count) // if overflow, go to 0 index
    //            {
    //                index = 0;
    //            }
    //        }

    //        ghost.movement.SetDirection(node.availableDirections[index]);
    //    }
    //}
}
