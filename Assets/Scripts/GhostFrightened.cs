using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    [SerializeField]
    private SpriteRenderer body;
    [SerializeField]
    private SpriteRenderer eyes;
    [SerializeField]
    private SpriteRenderer blue;
    [SerializeField]
    private SpriteRenderer white;

    [SerializeField]
    private GameObject ghostRetreat;

    public bool eaten { get; private set; }

    public override void Enable(float duration)
    {
        base.Enable(duration);

        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        Invoke(nameof(Flash), duration / 2f);
    }

    public override void Disable()
    {
        base.Disable();

        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            white.enabled = true;
            white.GetComponent<AnimatedSprite>().Restart();
        }
    }

    private void Eaten()
    {
        eaten = true;

        //Vector3 position = ghost.home.inside.position;
        //position.z = ghost.transform.position.z;
        //ghost.transform.position = position;

        // ghost.home.Enable(duration);

        //body.enabled = true;
        //eyes.enabled = true;
        //blue.enabled = false;
        //white.enabled = false;

        //this.gameObject.SetActive(false);
        //ghostRetreat.transform.position = transform.position;
        //ghostRetreat.SetActive(true);
        //ghostRetreat.GetComponent<GhostRetreat>().enabled = true;

        FindObjectOfType<GameManager>().CallEatGhostSequence(this.ghost, ghostRetreat);
    }

    private void OnEnable()
    {
        ghost.movement.speedMultiplier = 0.5f;
        eaten = false;
    }

    private void OnDisable()
    {
        ghost.movement.speedMultiplier = 1.0f;
        eaten = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (this.enabled)
            {
                Eaten();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // ghost makes random turns at each intersection
    {
        Node node = collision.GetComponent<Node>();

        if (node != null && this.enabled)
        {
            int index = Random.Range(0, node.availableDirections.Count);

            if (node.availableDirections[index] == -ghost.movement.direction && node.availableDirections.Count > 1) // making sure ghost doesn't backtrack and look stupid
            {
                index++;

                if (index >= node.availableDirections.Count) // if overflow, go to 0 index
                {
                    index = 0;
                }
            }

            ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Node node = collision.GetComponent<Node>();

    //    if (node != null && this.enabled)
    //    {
    //        Vector2 direction = Vector2.zero;
    //        float maxDistance = float.MinValue;

    //        foreach (Vector2 availableDirection in node.availableDirections)
    //        {
    //            Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y, 0.0f);
    //            float distance = (ghost.target.position - newPosition).sqrMagnitude; // calculate distance from possible direction to target being chased. sqrMagnitude better for performance than magnitude

    //            if (distance > maxDistance) // if this path is shorter, set direction to this direction
    //            {
    //                direction = availableDirection;
    //                maxDistance = distance;
    //            }
    //        }

    //        ghost.movement.SetDirection(direction);
    //    }
    //}
}
