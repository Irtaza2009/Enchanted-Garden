using UnityEngine;

public class MushroomSpawner : MonoBehaviour
{
    public GameObject mushroomPrefab; // Prefab with MushroomHandler attached
    public int numberOfMushrooms = 5; // Number of mushrooms to spawn


    void Start()
    {
        SpawnMushrooms();
    }
    public void SpawnMushrooms()
    {
        for (int i = 0; i < numberOfMushrooms; i++)
        {
            Instantiate(mushroomPrefab, transform.position, Quaternion.identity);

        }
    }
}
