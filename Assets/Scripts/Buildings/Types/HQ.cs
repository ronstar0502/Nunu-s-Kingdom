using System.Collections.Generic;
using UnityEngine;

public class HQ : Building
{
    //main building can only be one and cannot be constructed
    [Header("Villagers General Info")]
    [SerializeField] private List<GameObject> unemployedVillagers;
    [SerializeField] private List<GameObject> proffesionVillagers;
    [SerializeField] private int[] maxVillagerPerLevel = new int[3];
    [SerializeField] private int maxVillagerAmount;
    [SerializeField] private int currentVillagerAmount;

    [Header("Villlagers deep info")]
    [SerializeField] private List<GameObject> farmers;
    [SerializeField] private List<GameObject> warriors;
    [SerializeField] private List<GameObject> archers;

    [SerializeField]private List<GuardTower> guardTowers;
    public Farm farm;
    //private VillageInfo villageInfoUI;

    private void Start()
    {
        maxVillagerAmount = maxVillagerPerLevel[buildingData.level-1];
        //villageInfoUI = FindObjectOfType<VillageInfo>();
        //villageInfoUI.InitInfo(maxVillagerAmount,player.GetPlayerData().seedAmount);
    }

    protected override void LevelUpBuilding()
    {
        base.LevelUpBuilding();
        maxVillagerAmount = maxVillagerPerLevel[buildingData.level-1];
        //villageInfoUI.SetVillagersAmountText(currentVillagerAmount, maxVillagerAmount);
    }
    public void AddUnemployedVillager(GameObject villager)
    {
        unemployedVillagers.Add(villager);
        currentVillagerAmount++;

        //villageInfoUI.SetUnemployedText(unemployedVillagers.Count);
        //villageInfoUI.SetVillagersAmountText(currentVillagerAmount, maxVillagerAmount);
        print($"total villager {currentVillagerAmount} / {maxVillagerAmount} and {unemployedVillagers.Count} are unemployed");
    }

    public void AddProffesionVillager(GameObject villager , string buildingName)
    {
        switch (buildingName)
        {
            case "Archery":
                archers.Add(villager);
                //villageInfoUI.SetArchersText(archers.Count,unemployedVillagers.Count);
                AssignArcherToGuardTower(villager);
                break;
            case "Blacksmith":
                warriors.Add(villager);
                //villageInfoUI.SetWarriorsText(warriors.Count, unemployedVillagers.Count);
                break;
            case "Farm":
                farmers.Add(villager);
                //villageInfoUI.SetFarmersText(farmers.Count, unemployedVillagers.Count);
                break;
        }
        proffesionVillagers.Add(villager);
        print($"total villager {currentVillagerAmount} / {maxVillagerAmount} and {proffesionVillagers.Count} are employed and {unemployedVillagers.Count} are unemployed");
    }

    public bool CanRecruitVillager()
    {
        return currentVillagerAmount < maxVillagerAmount;
    }

    public bool HasUnemployedVillager()
    {
        return unemployedVillagers.Count > 0;
    }

    public GameObject GetRandomUnemployed()
    {
        int randomIndex = Random.Range(0, unemployedVillagers.Count);
        return unemployedVillagers[randomIndex];
    }

    public void RemoveUnemployed(GameObject unemployed)
    {
        unemployedVillagers.Remove(unemployed);
        print($"now you have {unemployedVillagers.Count} unepmloyed villagers");
    }

    public void SetFarm(Farm farm)
    {
        this.farm = farm;
    }

    public void FarmSeeds()
    {
        farm.FarmSeeds();
        //villageInfoUI.SetSeedsText(player.GetPlayerData().seedAmount);
    }
    public void AddGuardTower(GuardTower guardTower)
    {
        guardTowers.Add(guardTower);
    }

    private void AssignArcherToGuardTower(GameObject archerToAssign) //method to tell the archer which guard tower to go to
    {
        GuardTower guardTower = GetAvailableGuardTower();
        if (guardTower == null)
        {
            print("cant assign archer due to no guard towers slots available");
            return;
        }
        Archer archer = archerToAssign.GetComponent<Archer>();
        archer.GoToGuardTower(guardTower);
    }

    /*private GuardTower GetGuardTowerToAssignTo() //method to get a guard tower to assign an archer that got recruited
    {
        if (CanAssignArcherToEveryGuardTower())
        {
            int randomTower = Random.Range(0,guardTowers.Count);          
            return guardTowers[randomTower];
        }
        else
        {
            for (int i = 0; i < guardTowers.Count; i++)
            {
                if (guardTowers[i].hasOpenSlots()) return guardTowers[i];
            }
        }      
        return null;
    }
    private bool CanAssignArcherToEveryGuardTower() //checking if i can assign to all available guard towers
    {
        for (int i = 0; i < guardTowers.Count; i++)
        {
            if (!guardTowers[i].hasOpenSlots()) return false;
        }
        return true;
    }*/

    private GuardTower GetAvailableGuardTower()
    {
        List<GuardTower> availableTowers = GetGuardTowerForArcher();
        if (availableTowers.Count == 0)
        {
            print("no available towers");
            return null;
        }
        int randomTower = Random.Range(0, availableTowers.Count);
        return availableTowers[randomTower];
    }
    
    private List<GuardTower> GetGuardTowerForArcher()
    {
        List<GuardTower> availableTowers = new List<GuardTower>();
        for (int i = 0; i < guardTowers.Count; i++)
        {
            if (guardTowers[i].hasOpenSlots())
            {
                availableTowers.Add(guardTowers[i]);
            }
        }
        return availableTowers;

    }

    public GameObject GetRandomArcher() //for guard tower that just got built
    {
        for (int i = 0;i< archers.Count; i++)
        {
            if (!archers[i].GetComponent<Archer>().isAssigned)
            {
                return archers[i];
            }
        }   
        return null;
    }
    public int ArcherCount()
    {
        return archers.Count;
    }

    public List<GameObject> GetFarmersList()
    {
        return farmers;
    }

}

