using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : ProffesionBuilding
{
    [Header("Farm Details")]
    [SerializeField] private float harvestRate;
    [SerializeField] private int[] farmerSlots = new int[3];
    private int currentFarmerSlots;
    private int currentWorkingVillagers;
    private int harvestAmount;

    private void Start()
    {
        currentFarmerSlots = farmerSlots[0];
        harvestAmount = 1; //default harvest amount for idle = no farmers
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RecruitVillagerProffesion();
        }
    }

    /*public void ActivateFarm() // method to access from game manager to activate the FarmSeeds enumerator during day;
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
    }*/

    public void FarmSeeds()
    {
        harvestAmount = 1 * buildingData.buildingLevel + currentWorkingVillagers ;
        print($"harvest amount : {harvestAmount}");
        player.GetPlayerData().AddSeedsAmount(harvestAmount);
    }

    public override void RecruitVillagerProffesion() //add farmer if there is an available slot 
    {
        if (!IsFarmFull())
        {
            base.RecruitVillagerProffesion();            
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
