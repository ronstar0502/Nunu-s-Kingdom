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
    private bool playerInRange;

    protected override void Start()
    {
        base.Start();
        HQ.SetFarm(gameObject.GetComponent<Farm>());

        currentFarmerSlots = farmerSlots[0];
        harvestAmount = 1;        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            RecruitVillagerProffesion();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    public void FarmSeeds() //farm seeds
    {
        harvestAmount = (5 + currentWorkingVillagers) * buildingData.level;
        print($"harvest amount : {harvestAmount}");
        if (player != null)
        {
            player.GetPlayerData().AddSeedsAmount(harvestAmount);
            HQ.villageInfoUI.SetSeedsText();
        }
        else
        {
            print("player not found!!");
        }
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
        currentFarmerSlots = farmerSlots[buildingData.level - 1];
    }

    protected override void ChangeVillagerProffesion() //method to change unemployed to farmer
    {
        int randomSpawnPoint = Random.Range(0, villagerRecruitSpawnPoints.Length);
        GameObject proffesionVillager = Instantiate(villlagerProffesionPrefab, villagerRecruitSpawnPoints[randomSpawnPoint].position, Quaternion.identity, villagerRecruitSpawnPoints[randomSpawnPoint]);
        HQ.AddProffesionVillager(proffesionVillager, buildingData.buildingName);
    }



}
