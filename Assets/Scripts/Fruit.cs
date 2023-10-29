// Import the necessary libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define a class named "Fruit" that inherits from MonoBehaviour
public class Fruit : MonoBehaviour
{
    // Serialize a private field for the time it takes for the fruit to despawn
    [SerializeField]
    private float despawnTime = 8f;

    // Set a public variable for the points awarded when the fruit is eaten
    public int points = 100;

    // This method is called when the object is created
    private void Start()
    {
        // Destroy the game object after the specified despawn time
        Object.Destroy(gameObject, despawnTime);
    }

    // This method can be overridden by subclasses to handle eating the fruit
    protected virtual void Eat()
    {
        // Find the GameManager and play the fruit eating sound
        FindObjectOfType<GameManager>().PlayFruitSound();
        
        // Find the GameManager and inform it that the fruit has been eaten
        FindObjectOfType<GameManager>().FruitEaten(this);
    }

    // This method is called when the object collides with a 2D collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with an object on the "Pacman" layer
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            // Call the Eat() method to handle the fruit being eaten
            Eat();
        }
    }
}
