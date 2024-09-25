using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject flowerPrefab;
    [SerializeField] private float spawnLeftBorder;
    [SerializeField] private float spawnRightBorder;
    [SerializeField] private float minSpawnDelay;
    [SerializeField] private float maxSpawnDelay;
    public bool isNight;
    public IEnumerator SpawnFlowers()
    {
        isNight = true;
        while (isNight)
        {
            float spawnDelay = Random.Range(minSpawnDelay,maxSpawnDelay); 
            yield return new WaitForSeconds(spawnDelay);
            SpawnFlower();
        }
    }
    private void SpawnFlower()
    {
        float randomXPos = Random.Range(spawnLeftBorder, spawnRightBorder);
        Vector2 spawnPos = new Vector2(randomXPos, transform.position.y);
        GameObject flower = Instantiate(flowerPrefab, spawnPos,Quaternion.identity,transform);
    }

}
