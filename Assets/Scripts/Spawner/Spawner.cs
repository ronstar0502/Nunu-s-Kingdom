using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    private Vector3 spawnPosition;
    private int randomNum;
    [SerializeField] private Wave[] waves;
    private int currWave = 0;
    public void StartSpawning()
    {
        int amount = waves[currWave].enemiesAmount[0];
        for (int i = 0; i < waves[currWave].enemies.Length; i++)
        {
            for (int j = 0; j < waves[currWave].enemiesAmount[i]; j++)
            {
                randomNum=Random.Range(0, 2);
                spawnPosition = spawnPoints[randomNum].position;
                Instantiate(waves[currWave].enemies[i], spawnPosition, Quaternion.identity, spawnPoints[randomNum]);
            }
        }
        if(currWave == waves.Length - 1)
        {
            return;
            //win
        }
        currWave++;
    }
}
