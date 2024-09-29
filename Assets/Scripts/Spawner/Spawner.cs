using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints; //portals
    [SerializeField] private GameObject[] enemyDirectionIndicators;
    [SerializeField] private Wave[] waves;
    [SerializeField] private float spawnDelay;
    private Vector3 _spawnPosition;
    private int _randomSpawnPoint;
    private int _currWave = 0;

    public IEnumerator StartSpawning()
    {
        _randomSpawnPoint = Random.Range(0, 2);

        enemyDirectionIndicators[_randomSpawnPoint].SetActive(true);
        DisableNotActivePortal();
        if (_currWave < waves.Length)
        {
            for (int i = 0; i < waves[_currWave].enemies.Length; i++)
            {
                for (int j = 0; j < waves[_currWave].enemiesAmount[i]; j++)
                {

                    _spawnPosition = spawnPoints[_randomSpawnPoint].position;
                    Instantiate(waves[_currWave].enemies[i], _spawnPosition, Quaternion.identity, spawnPoints[_randomSpawnPoint]);
                    yield return new WaitForSeconds(spawnDelay);
                }
            }
        }
        _currWave++;
        yield return new WaitForSeconds(1.5f);
        enemyDirectionIndicators[_randomSpawnPoint].SetActive(false);
    }

    private void DisableNotActivePortal()
    {
        if(_randomSpawnPoint == 0)
        {
            spawnPoints[1].Find("Portal").gameObject.SetActive(false);
        }
        else if(_randomSpawnPoint == 1)
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
