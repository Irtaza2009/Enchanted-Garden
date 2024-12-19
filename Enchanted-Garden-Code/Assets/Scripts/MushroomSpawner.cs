using UnityEngine;
using System.Collections;

public class MushroomSpawner : MonoBehaviour
{
    public GameObject mushroomPrefab; // Prefab with MushroomHandler attached
    public int numberOfMushrooms = 5; // Number of mushrooms to spawn
    public float spawnInterval = 120f; // Time in seconds between spawns

    void Start()
    {
        StartCoroutine(SpawnMushroomsPeriodically());
    }

    private IEnumerator SpawnMushroomsPeriodically()
    {
        while (true)
        {
            SpawnMushrooms();
            yield return new WaitForSeconds(spawnInterval);
        }
    }


   
    public void SpawnMushrooms()
    {
        for (int i = 0; i < numberOfMushrooms; i++)
        {
            Instantiate(mushroomPrefab, transform.position, Quaternion.identity);

        }
    }
}
