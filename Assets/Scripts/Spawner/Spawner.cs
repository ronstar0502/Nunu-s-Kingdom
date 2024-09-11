using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnerPortalPrefab;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] private GameObject[] enemyDirectionIndicators; //place holder
    private Vector3 spawnPosition;
    private int randomSpawnPoint;
    [SerializeField] private Wave[] waves;
    private int currWave = 0;

    public IEnumerator StartSpawning()
    {
        int amount = waves[currWave].enemiesAmount[0]; // ??
        randomSpawnPoint = Random.Range(0, 2);
        GameObject spawnerPortal = Instantiate(spawnerPortalPrefab, spawnPoints[randomSpawnPoint]);

        enemyDirectionIndicators[randomSpawnPoint].SetActive(true); //place holder
        if(currWave < waves.Length)
        {
            for (int i = 0; i < waves[currWave].enemies.Length; i++)
            {
                for (int j = 0; j < waves[currWave].enemiesAmount[i]; j++)
                {

                    spawnPosition = spawnPoints[randomSpawnPoint].position;
                    Instantiate(waves[currWave].enemies[i], spawnPosition, Quaternion.identity, spawnPoints[randomSpawnPoint]);
                    yield return new WaitForSeconds(2);
                }
            }
        }
        //Destroy(spawnerPortal); //place holder
        currWave++;
    }
}
