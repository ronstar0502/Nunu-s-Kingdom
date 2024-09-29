using System.Collections;
using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField] private GameObject seedPrefab;
    [SerializeField] private Transform seedSpawnTransform;
    [SerializeField] private int minSpawnSeedAmount;
    [SerializeField] private int maxSpawnSeedAmount;
    [SerializeField] private int seedMinValue;
    [SerializeField] private int seedMaxValue;
    private int _seedsToSpawn;

    private void Start()
    {
        SetSeedsToSpawn();
    }

    private void SetSeedsToSpawn()
    {
        int randomAmount = Random.Range(minSpawnSeedAmount, maxSpawnSeedAmount+1);
        _seedsToSpawn = randomAmount;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(SpawnSeeds());
        }
    }

    private IEnumerator SpawnSeeds()
    {
        for (int i = 0; i < _seedsToSpawn; i++)
        {
            GameObject seed = Instantiate(seedPrefab, seedSpawnTransform.position,Quaternion.identity);
            int seedValue = Random.Range(seedMinValue,seedMaxValue);
            seed.GetComponent<Seed>().InitSeed(seedValue);
            print($"spawned seed , {seed.name}");
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

}
