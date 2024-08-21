using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : Building
{
    //villager slots
    //generate x amount of seeds every x amount of seconds
    //amount of seeds depends on farm level , villager amount working in the farm

    [SerializeField] private GameObject farmerPrefab;
    [SerializeField] private float harvestRate;
    private int[] farmerSlots = new int[3];
    private int currentFarmerSlots;
    private int currentWorkingVillagers;

    private void Start()
    {
        currentFarmerSlots = farmerSlots[0];
    }

    public void ActivateFarm() // method to access from game manager to activate the FarmSeeds enumerator during day;
    {
        StartCoroutine(FarmSeeds());
    }

    public void DeactivateFarm() //method method to access from game manager to deactivate the FarmSeeds enumerator during night;
    {
        StopCoroutine(FarmSeeds());
    }
    private IEnumerator FarmSeeds() //enumerator to harvest seeds every x amount of time
    {
        while(true)
        {
            player.GetPlayerData().AddSeedsAmount(3); // change later to currentWorkingVillagers for the amount of villagers
            print($"seeds amount {player.GetPlayerData().seedAmount}");
            yield return new WaitForSeconds(harvestRate);
        }
    }

    public void AddFarmer() //add farmer if there is an available slot // placeholder method
    {
        if (!IsFarmFull())
        {
            currentWorkingVillagers++;
        }
    }

    public bool IsFarmFull() //checks if there are no avalable slots
    {
        return currentWorkingVillagers == currentFarmerSlots;
    }

    protected override void LevelUpBuilding()
    {
        base.LevelUpBuilding();
        currentFarmerSlots = farmerSlots[buildingData.buildingLevel-1];
    }



}
