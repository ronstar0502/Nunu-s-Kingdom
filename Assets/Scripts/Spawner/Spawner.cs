using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    private Vector3 spawnPosition;
    [SerializeField] private Wave[] waves;
    private int currWave = 0;
    public void StartSpawning()
    {
        spawnPosition = spawnPoints[Random.Range(0, 2)].position;
        int amount = waves[currWave].enemiesAmount[0];
        for (int i = 0; i < waves[currWave].enemies.Length; i++)
        {
            for (int j = 0; j < waves[currWave].enemiesAmount[i]; j++)
            {
                spawnPosition = spawnPoints[Random.Range(0, 2)].position;
                Instantiate(waves[currWave].enemies[i], spawnPosition, Quaternion.identity);
            }
        }
        currWave++;
    }
}
