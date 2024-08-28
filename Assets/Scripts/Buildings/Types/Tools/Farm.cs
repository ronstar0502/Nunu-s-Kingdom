using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farm : ProffesionBuilding
{
    [Header("Farm Details")]
    [SerializeField] private int[] farmerSlots = new int[3];
    private int currentFarmerSlots;
    [SerializeField] private int currentWorkingVillagers;
    private int harvestAmount;

    //TBD: visualization of farmers

    private void Start()
    {
        HQ HQ = FindObjectOfType<HQ>();
        HQ.SetFarm(gameObject.GetComponent<Farm>());

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
    public void FarmSeeds()
    {
        harvestAmount = 1 * buildingData.level + currentWorkingVillagers ;
        print($"harvest amount : {harvestAmount}");
        if (player != null)
        {
            player.GetPlayerData().AddSeedsAmount(harvestAmount);
        }
        else
        {
            print("player not found!!");
        }
    }

    /*public override void RecruitVillagerProffesion() //add farmer if there is an available slot 
    {
        if (!IsFarmFull())
        {
            base.RecruitVillagerProffesion();            
        }
    }*/
    public bool IsFarmFull() //checks if there are no avalable slots
    {
        return currentWorkingVillagers == currentFarmerSlots;
    }

    protected override void LevelUpBuilding()
    {
        base.LevelUpBuilding();
        currentFarmerSlots = farmerSlots[buildingData.level-1];
    }



}
