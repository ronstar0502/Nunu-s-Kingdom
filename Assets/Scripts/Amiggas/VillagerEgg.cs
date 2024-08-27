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
    private int eggSlot;

    private void Start()
    {
        hatchTimer = eggHatchTimer;
        hatchery = FindObjectOfType<Hatchery>();
    }

    public void InitEgg(GameObject villager,Transform spawnPoint , int eggSlot) //initializing egg data to know what to spawn after it hatches and where;
    {
        _villager = villager;
        villagerSpawnPoint = spawnPoint;
        this.eggSlot = eggSlot;
    }

    private void Update()
    {
        hatchTimer -= Time.deltaTime;
        CanEggHatch();
    }

    private void CanEggHatch() //method to check if the egg can hatch
    {
        if (hatchTimer <= 0f)
        {
            GameObject villagerObj = Instantiate(_villager, villagerSpawnPoint.position, Quaternion.identity); // after the egg timer is over , spawns the villager
            hatchery.EggHatch(villagerObj, eggSlot);
            Destroy(gameObject); //after the villager spawn destroy egg
        }
    }
}
