using System.Collections;
using UnityEngine;

public class Script : MonoBehaviour
{
    public GameObject firePrefab;
    public float spawnInterval = 0.2f;
    public float spawnRangeX = 8f;
    public float spawnRangeY = 4f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnFireRandomLocation();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnFireRandomLocation()
    {
        Vector2 randomPosition = new Vector2(Random.Range(-spawnRangeX, spawnRangeX), Random.Range(-spawnRangeY, spawnRangeY));

        Instantiate(firePrefab, randomPosition, Quaternion.identity);
    }
}