using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQ : Building
{
    //main building can only be one and cannot be constructed
    [Header("Villagers General Info")]
    [SerializeField] private List<GameObject> unemployedAmmigas;
    [SerializeField] private List<GameObject> proffesionAmmigas;
    [SerializeField] private int[] maxAmmigasPerLevel = new int[3];
    [SerializeField] private int maxAmiggaAmount;
    [SerializeField] private int currentAmiggaAmount;

    [Header("Villlagers deep info")]
    [SerializeField] private List<GameObject> farmers;
    [SerializeField] private List<GameObject> warriors;
    [SerializeField] private List<GameObject> archers;

    [SerializeField]private List<GuardTower> guardTowers;
    public Farm farm;
    public VillageInfo villageInfoUI;
    public bool isNightMode;
    private HealthUI healthUI;

    protected void Start()
    {
        healthUI = FindAnyObjectByType<HealthUI>();
        maxAmiggaAmount = maxAmmigasPerLevel[buildingData.level-1];
        villageInfoUI = GetComponent<VillageInfo>();
        villageInfoUI.InitInfo(maxAmiggaAmount,player.GetPlayerData().seedAmount);
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
        maxAmiggaAmount = maxAmmigasPerLevel[buildingData.level-1];
        villageInfoUI.SetVillagersAmountText(currentAmiggaAmount, maxAmiggaAmount);
        villageInfoUI.SetSeedsText();
        healthUI.SetHealthBar(buildingHealth,buildingData.health);
    }
    public void AddUnemployedVillager(GameObject villager)
    {
        unemployedAmmigas.Add(villager);
        villager.transform.SetParent(gameObject.transform);
        villageInfoUI.SetUnemployedText(unemployedAmmigas.Count);
        villageInfoUI.SetVillagersAmountText(currentAmiggaAmount, maxAmiggaAmount);
        print($"total villager {currentAmiggaAmount} / {maxAmiggaAmount} and {unemployedAmmigas.Count} are unemployed");
    }

    public void AddToTotalVillagerAmount()
    {
        currentAmiggaAmount++;
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(1);
        healthUI.SetHealthBar(buildingHealth,buildingData.health);
    }

    public void AddProffesionVillager(GameObject villager , string buildingName)
    {
        switch (buildingName)
        {
            case "Archery":
                archers.Add(villager);
                villageInfoUI.SetArchersText(archers.Count,unemployedAmmigas.Count);
                AssignArcherToGuardTower(villager);
                break;
            case "Blacksmith":
                warriors.Add(villager);
                villageInfoUI.SetWarriorsText(warriors.Count, unemployedAmmigas.Count);
                break;
            case "Farm":
                farmers.Add(villager);
                villageInfoUI.SetFarmersText(farmers.Count, unemployedAmmigas.Count);
                break;
        }
        proffesionAmmigas.Add(villager);
        print($"total villager {currentAmiggaAmount} / {maxAmiggaAmount} and {proffesionAmmigas.Count} are employed and {unemployedAmmigas.Count} are unemployed");
    }

    public bool CanRecruitVillager()
    {
        return currentAmiggaAmount < maxAmiggaAmount;
    }

    public bool HasUnemployedVillager()
    {
        return unemployedAmmigas.Count > 0;
    }

    public GameObject GetRandomUnemployed() //gets a random unemployed to recruit to a proffesion
    {
        for (int i = 0; i < unemployedAmmigas.Count; i++)
        {
            if (!unemployedAmmigas[i].GetComponent<Villager>().isProffesionRecruited)
            {
                return unemployedAmmigas[i];
            }
        }
        return null;
        //int randomIndex = Random.Range(0, unemployedAmmigas.Count);
        //return unemployedAmmigas[randomIndex];
    }

    public void RemoveUnemployed(GameObject unemployed) // after recruitment of an unemployed remove from the list
    {
        unemployedAmmigas.Remove(unemployed);
        print($"now you have {unemployedAmmigas.Count} unepmloyed villagers");
    }

    public void RemoveArcher(GameObject Archer) // after recruitment of an unemployed remove from the list
    {
        archers.Remove(Archer);
        currentAmiggaAmount--;
        villageInfoUI.SetVillagersAmountText(currentAmiggaAmount,maxAmiggaAmount);
        print($"now you have {archers.Count} archers");
    }

    public void SetFarm(Farm farm)
    {
        this.farm = farm;
    }

    public void FarmSeeds()
    {
        farm.FarmSeeds();

    }
    public void AddGuardTower(GuardTower guardTower) //list of built guard tower
    {
        guardTowers.Add(guardTower);
        print($"guard tower count: {guardTowers.Count}");
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
                warriors[i].GetComponent<CombatVillager>().ChangeToPatrolMode();
            }
        }
        for (int i = 0; i < archers.Count; i++)
        {
            if (archers[i] != null)
            {
                archers[i].GetComponent<CombatVillager>().ChangeToPatrolMode();
            }
        }
    }
}

