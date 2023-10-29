using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRetreat : MonoBehaviour
{
    [SerializeField]
    private Transform inside;
    [SerializeField]
    private Transform outside;

    [SerializeField]
    private Ghost ghost;

    [HideInInspector]
    public Movement movement;

    private List<Vector2> availableInitialDirections;
    [SerializeField]
    private LayerMask obstacleLayer;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        if (this.enabled && Vector2.Distance(transform.position, outside.position) < 1)
        {
            StartCoroutine(EnterTransition());
        }
    }

    private void OnEnable()
    {
        availableInitialDirections = new List<Vector2>();

        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);

        if (this.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            foreach (Vector2 availableDirection in availableInitialDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                float distance = (outside.position - newPosition).sqrMagnitude; // calculate distance from possible direction to target being chased. sqrMagnitude better for performance than magnitude

                //if (availableDirection == -movement.direction && node.availableDirections.Count > 1) // prevents ghost from backtracking
                //{

                //}
                //else
                //{
                    if (distance < minDistance) // if this path is shorter, set direction to this direction
                    {
                        direction = availableDirection;
                        minDistance = distance;
                    }
                //}
            }
            
            movement.SetDirection(direction, true);
            // Debug.Log(direction);
        }
    }

    private void CheckAvailableDirection(Vector2 direction) // checks to see if wall in way of direction. If not, then add direction to list of possible directions
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0.0f, direction, 1.0f, obstacleLayer); // pos, size of box, angle, direction, distance, layer to hit

        if (hit.collider == null)
        {
            availableInitialDirections.Add(direction);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Ghost will head for outside of home
    {
        Node node = collision.GetComponent<Node>();

        if (node != null && this.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                float distance = (outside.position - newPosition).sqrMagnitude; // calculate distance from possible direction to target being chased. sqrMagnitude better for performance than magnitude

                if (availableDirection == -movement.direction && node.availableDirections.Count > 1) // prevents ghost from backtracking
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
            
            movement.SetDirection(direction);
        }
    }

    private IEnumerator EnterTransition() // allow for passing through walls, once inside home disable passing through walls & enable movement script
    {
        movement.SetDirection(Vector2.down, true);
        movement.rigidBody.isKinematic = true;
        movement.enabled = false;

        Vector3 position = transform.position;

        float duration = 0.1f;
        float elapsed = 0.0f;

        while (elapsed < duration) // Loop handles animation to outside of home
        {
            Vector3 newPosition = Vector3.Lerp(position, outside.position, elapsed / duration);
            newPosition.z = position.z;
            transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null; // waits 1 frame
        }

        duration = 0.2f;
        elapsed = 0.0f;

        while (elapsed < duration) // Loop handles animation from outside of home to inside of home
        {
            Vector3 newPosition = Vector3.Lerp(outside.position, inside.position, elapsed / duration);
            newPosition.z = position.z;
            transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null; // waits 1 frame
        }

        movement.SetDirection(new Vector2(Random.value < 0.5f ? -1.0f : 1.0f, 0.0f), true); // randomly go left or right once out of home
        movement.rigidBody.isKinematic = false;
        movement.enabled = true;

        ghost.ResetState();
        ghost.home.Enable(0.5f);
        ghost.gameObject.transform.position = transform.position;
        ghost.gameObject.SetActive(true);

        this.gameObject.SetActive(false);
        this.enabled = false;
    }
}
