using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChase : GhostBehavior // chase after player, when chase time is up transition to scatter state
{
    [SerializeField]
    private GameManager gm;

    private void OnDisable() // transistion to scatter state
    {
        //Debug.Log("Scatter: " + gm.CalculateScatterTime());
        ghost.scatter.Enable(gm.CalculateScatterTime());
        //ghost.scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Node node = collision.GetComponent<Node>();

        if (node != null && this.enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                float distance = (ghost.target.position - newPosition).sqrMagnitude; // calculate distance from possible direction to target being chased. sqrMagnitude better for performance than magnitude

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
}
