using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerEgg : MonoBehaviour
{
    [SerializeField] private float eggHatchTimer;
    private Hatchery hatchery;
    private GameObject _villager;
    private Transform villagerSpawnPoint;
    private float hatchTimer;

    private void Start()
    {
        hatchTimer = eggHatchTimer;
        hatchery = FindObjectOfType<Hatchery>();
    }

    public void InitEgg(GameObject villager,Transform spawnPoint )
    {
        _villager = villager;
        villagerSpawnPoint = spawnPoint;
    }

    private void Update()
    {
        hatchTimer -= Time.deltaTime;
        if (hatchTimer <= 0f)
        {
            GameObject villagerObj = Instantiate(_villager, villagerSpawnPoint.position ,Quaternion.identity);
            hatchery.EggHatch();
            Destroy(gameObject);
        }
    }
}
