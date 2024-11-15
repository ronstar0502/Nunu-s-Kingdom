using System.Collections.Generic;
using UnityEngine;

public class HQ : Building
{
    //main building can only be one and cannot be constructed
    [Header("Villagers General Info")]
    [SerializeField] private List<GameObject> unemployedVillagers;
    [SerializeField] private List<GameObject> proffesionVillagers;
    [SerializeField] private int[] maxVillagersPerLevel = new int[3];
    [SerializeField] private int maxVillagerAmount;
    [SerializeField] private int currentVillagerAmount;

    [Header("Villlagers deep info")]
    [SerializeField] private List<GameObject> farmers;
    [SerializeField] private List<GameObject> warriors;
    [SerializeField] private List<GameObject> archers;

    [Header("Other info")]
    [SerializeField]private List<GuardTower> guardTowers;
    public Transform leftPatrolBorderTransform , rightPatrolBorderTransform ; //for X axsis
    public List<Farm> farms;
    public bool isNightMode;
    private HealthUI _healthUI;

    protected void Start()
    {
        _healthUI = FindAnyObjectByType<HealthUI>();
        maxVillagerAmount = maxVillagersPerLevel[buildingData.level-1];
        villageInfoUI.InitInfo(maxVillagerAmount, player.GetPlayerData().seedAmount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(1);
        }
    }

    protected override void LevelUpBuilding()
    {
        base.LevelUpBuilding();
        maxVillagerAmount = maxVillagersPerLevel[buildingData.level-1];
        villageInfoUI.SetVillagersAmountText(currentVillagerAmount, maxVillagerAmount);
        villageInfoUI.SetSeedsText(true);
        _healthUI.SetHealthBar(buildingHealth,buildingData.health);
    }

    public void AddUnemployedVillager(GameObject villager)
    {
        unemployedVillagers.Add(villager);
        villager.transform.SetParent(gameObject.transform);
        villageInfoUI.SetUnemployedText(unemployedVillagers.Count);
        villageInfoUI.SetVillagersAmountText(currentVillagerAmount, maxVillagerAmount);
        print($"total villager {currentVillagerAmount} / {maxVillagerAmount} and {unemployedVillagers.Count} are unemployed");
    }

    public void AddToTotalVillagerAmount()
    {
        currentVillagerAmount++;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(1);
        _healthUI.SetHealthBar(buildingHealth,buildingData.health);
    }

    public void AddProffesionVillager(GameObject villager , string buildingName)
    {
        switch (buildingName)
        {
            case "Archery":
                archers.Add(villager);
                villageInfoUI.SetArchersText(archers.Count,unemployedVillagers.Count);
                AssignArcherToGuardTower(villager);
                break;
            case "Blacksmith":
                warriors.Add(villager);
                villageInfoUI.SetWarriorsText(warriors.Count, unemployedVillagers.Count);
                break;
            case "Farm":
                farmers.Add(villager);
                villageInfoUI.SetFarmersText(farmers.Count, unemployedVillagers.Count);
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

    public GameObject GetRandomUnemployed() //gets a random unemployed to recruit to a proffesion
    {
        for (int i = 0; i < unemployedVillagers.Count; i++)
        {
            if (!unemployedVillagers[i].GetComponent<Villager>().isProffesionRecruited)
            {
                return unemployedVillagers[i];
            }
        }
        return null;
    }

    public void RemoveUnemployed(GameObject unemployed) // after recruitment of an unemployed remove from the list
    {
        unemployedVillagers.Remove(unemployed);
    }

    public void RemoveArcher(GameObject Archer) // after recruitment of an unemployed remove from the list
    {
        archers.Remove(Archer);
        currentVillagerAmount--;
        villageInfoUI.SetVillagersAmountText(currentVillagerAmount,maxVillagerAmount);
    }
    public void RemoveFarm(Farm farm)
    {
        farms.Remove(farm);
    }

    public void RemoveGuardTower(GuardTower tower)
    {
        guardTowers.Remove(tower);
    }

    public void AddFarm(Farm farm)
    {
        farms.Add(farm);
    }

    public void FarmSeeds()
    {
        foreach (Farm farm in farms)
        {
            if (farm != null)
            {
                farm.FarmSeeds();
            }
        }

    }

    public void AddGuardTower(GuardTower guardTower) //list of built guard tower
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
        archer.GoToAssignedGuardTower(guardTower);
    }

    private GuardTower GetAvailableGuardTower() //method to get one of the available guard towers for the archer
    {
        List<GuardTower> availableTowers = GetAvailableGuardTowers();
        if (availableTowers.Count == 0)
        {
            print("no available towers");
            return null;
        }
        int randomTower = Random.Range(0, availableTowers.Count);
        return availableTowers[randomTower];
    }
    
    private List<GuardTower> GetAvailableGuardTowers() //method to get all available guard tower that can assign an archer to
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
            if (!archers[i].GetComponent<Archer>().isAssignedToGuardTower)
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

    public void SetAmmigasToCombatMode()
    {
        isNightMode = true;
        for(int i = 0; i < warriors.Count; i++)
        {
            warriors[i].GetComponent<CombatVillager>().ChangeToCombatMode();
        }
        for (int i = 0; i < archers.Count; i++)
        {
            archers[i].GetComponent<CombatVillager>().ChangeToCombatMode();
        }
    }

    public void SetAmmigastToPatrolMode()
    {
        isNightMode = false;
        for (int i = 0; i < warriors.Count; i++)
        {
            if (warriors[i] != null)
            {
                warriors[i].GetComponent<CombatVillager>().ChangeToDayMode();
            }
        }
        for (int i = 0; i < archers.Count; i++)
        {
            if (archers[i] != null)
            {
                archers[i].GetComponent<CombatVillager>().ChangeToDayMode();
            }
        }
    }
}

