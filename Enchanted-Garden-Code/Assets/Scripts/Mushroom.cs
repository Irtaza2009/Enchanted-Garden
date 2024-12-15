using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{

    public Sprite[] mushroomSprites; // Array to hold your 3 mushroom sprites
    public float spawnRangeX = 11.5f; // Horizontal spawn range
    public float spawnRangeY = 3.5f; // Vertical spawn range
    public int mushroomValue = 1; // Number of mushrooms awarded per hit

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get or add a SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (mushroomSprites.Length > 0)
        {
            // Randomly assign one of the mushroom sprites
            spriteRenderer.sprite = mushroomSprites[Random.Range(0, mushroomSprites.Length)];
        }

        // Randomly place the mushroom within the defined spawn area
        Vector3 randomPosition = new Vector3(
            Random.Range(-spawnRangeX, spawnRangeX),
            Random.Range(-spawnRangeY, spawnRangeY),
            0 
        );
        transform.position = randomPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player hits the mushroom
        if (other.CompareTag("Player"))
        {
            // Add mushrooms to the player's inventory
            GameManager.Instance.mushroomCount += mushroomValue;
            Debug.Log("Mushrooms: " + GameManager.Instance.mushroomCount);

            // Optionally play a sound or animation here before destroying the object
            Destroy(gameObject);
        }
    }
}
