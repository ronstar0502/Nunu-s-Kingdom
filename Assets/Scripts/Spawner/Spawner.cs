using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;
    private Vector3 spawnPosition;
    private int randomSpawnPoint;
    [SerializeField] private Wave[] waves;
    private int currWave = 0;
    public void StartSpawning()
    {
        int amount = waves[currWave].enemiesAmount[0];
        randomSpawnPoint = Random.Range(0, 2);
        for (int i = 0; i < waves[currWave].enemies.Length; i++)
        {
            for (int j = 0; j < waves[currWave].enemiesAmount[i]; j++)
            {
                
                spawnPosition = spawnPoints[randomSpawnPoint].position;
                Instantiate(waves[currWave].enemies[i], spawnPosition, Quaternion.identity, spawnPoints[randomSpawnPoint]);
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
