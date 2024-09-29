using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints; //portals
    [SerializeField] private GameObject[] enemyDirectionIndicators;
    [SerializeField] private Wave[] waves;
    [SerializeField] private float spawnDelay;
    private Vector3 spawnPosition;
    private int randomSpawnPoint;
    private int currWave = 0;

    public IEnumerator StartSpawning()
    {
        //int amount = waves[currWave].enemiesAmount[0]; // ??
        randomSpawnPoint = Random.Range(0, 2);

        enemyDirectionIndicators[randomSpawnPoint].SetActive(true);
        DisableNotActivePortal();
        if (currWave < waves.Length)
        {
            for (int i = 0; i < waves[currWave].enemies.Length; i++)
            {
                for (int j = 0; j < waves[currWave].enemiesAmount[i]; j++)
                {

                    spawnPosition = spawnPoints[randomSpawnPoint].position;
                    Instantiate(waves[currWave].enemies[i], spawnPosition, Quaternion.identity, spawnPoints[randomSpawnPoint]);
                    yield return new WaitForSeconds(spawnDelay);
                }
            }
        }
        currWave++;
        yield return new WaitForSeconds(1.5f);
        enemyDirectionIndicators[randomSpawnPoint].SetActive(false);
    }

    private void DisableNotActivePortal()
    {
        if(randomSpawnPoint == 0)
        {
            spawnPoints[1].Find("Portal").gameObject.SetActive(false);
        }
        else if(randomSpawnPoint == 1)
        {
            spawnPoints[0].Find("Portal").gameObject.SetActive(false);
        }
    }

    public void EnableAllPortals()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i].Find("Portal").gameObject.SetActive(true);
        }
    }

    public int GetWaveCount()
    {
        return waves.Length;
    }
}
