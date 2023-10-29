using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHome : GhostBehavior // handles ghost leaving home
{
    public Transform inside;
    public Transform outside;

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(ExitTransition());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // handles ghosts bobbing up & down inside home
    {
        if (this.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            ghost.movement.SetDirection(-ghost.movement.direction);
        }
    }

    private IEnumerator ExitTransition() // allow for passing through walls, once outside home disable passing through walls & enable movement script
    {
        ghost.movement.SetDirection(Vector2.up, true);
        ghost.movement.rigidBody.isKinematic = true;
        ghost.movement.enabled = false;

        Vector3 position = transform.position;

        float duration = 0.5f;
        float elapsed = 0.0f;

        while (elapsed < duration) // Loop handles animation to center of home
        {
            Vector3 newPosition = Vector3.Lerp(position, inside.position, elapsed / duration);
            newPosition.z = position.z;
            ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null; // waits 1 frame
        }

        elapsed = 0.0f;

        while (elapsed < duration) // Loop handles animation from center of home to outside of home
        {
            Vector3 newPosition = Vector3.Lerp(inside.position, outside.position, elapsed / duration);
            newPosition.z = position.z;
            ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null; // waits 1 frame
        }

        ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1.0f : 1.0f, 0.0f), true); // randomly go left or right once out of home
        ghost.movement.rigidBody.isKinematic = false;
        ghost.movement.enabled = true;
    }
}
