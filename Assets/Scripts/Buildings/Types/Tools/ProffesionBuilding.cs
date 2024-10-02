using System.Collections;
using UnityEngine;

public class ProffesionBuilding : Building
{
    [Header("Prefabs")]
    [SerializeField] protected GameObject villlagerProffesionPrefab;
    [Header("Transforms")]
    [SerializeField] protected Transform villagerRecruitSpot;
    [SerializeField] protected Transform[] villagerRecruitSpawnPoints;
    [Header("Other Data")]
    [SerializeField] private string toolName;
    [SerializeField] private float changeProffesionDelay;
    protected HQ HQ;
    protected Villager villager;

    protected virtual void Start()
    {
        HQ = FindObjectOfType<HQ>();
        villager = villlagerProffesionPrefab.GetComponent<Villager>();
    }
    protected override void LevelUpBuilding()
    {
        base.LevelUpBuilding();
    }

    protected override void RefreshPopUp()
    {
        // for proffesion building that can recruit
        buildingPopUp.GetComponent<BuildingPopUp>().EnableBuildingPopUp(nextLevelCost, GetRecruitCost(), CanRecruit(), buildingData.level);

    }
    public virtual void RecruitVillagerProffesion() //method for recruiting unemployed to the specific proffesion 
    {
        if (HQ == null)
        {
            print("HQ is not found");
        }
        if (villager == null)
        {
            print("Villager data not found");
        }
        if (HQ.HasUnemployedVillager())
        {
            if (player.GetPlayerData().seedAmount >= GetRecruitCost())
            {
                if (VillagerRecruitAction())
                {
                    player.GetPlayerData().SubstarctSeedsAmount(GetRecruitCost());
                    villageInfoUI.SetSeedsText(true);
                    InvokeBuildingStateChanged();
                }
            }
            else
            {
                print($"has unemployed villager but not enough seeds , you have {player.GetPlayerData().seedAmount} and you need {GetRecruitCost()}");
            }
        }
        else
        {
            print("no unemployed villager available, pls recruit more villager from the hatchery");
        }
    }
    private bool VillagerRecruitAction() //method for the recruit an unemployed villager to a proffesion
    {
        GameObject randomUnemployed = HQ.GetRandomUnemployed();
        if (randomUnemployed != null)
        {
            Villager unemployedVillager = randomUnemployed.GetComponent<Villager>();
            unemployedVillager.isProffesionRecruited = true;
            StartCoroutine(unemployedVillager.ChangeVillagerProffesion(gameObject, transform.position));
            return true;
        }
        return false;

    }

    private bool CanRecruit()
    {
        return player.GetPlayerData().seedAmount >= GetRecruitCost() && HQ.HasUnemployedVillager();
    }
    public void VillagerProffesionChange_OnArrival(GameObject unemployedVillager) //destroying the unepmloyed and invoking for a delay of x amount of time to change proffesion
    {
        HQ.RemoveUnemployed(unemployedVillager);
        RefreshPopUp();
        Invoke(nameof(ChangeVillagerProffesion), changeProffesionDelay);
        Destroy(unemployedVillager);
    }

    public int GetRecruitCost()
    {
        return villager.GetVillagerData().seedsCost;
    }
    protected virtual void ChangeVillagerProffesion() // method to change unemployed to the building's proffesion
    {
        int randomSpawnPoint = Random.Range(0, villagerRecruitSpawnPoints.Length);
        GameObject proffesionVillager = Instantiate(villlagerProffesionPrefab, villagerRecruitSpawnPoints[randomSpawnPoint].position, Quaternion.identity);
        HQ.AddProffesionVillager(proffesionVillager, buildingData.buildingName);

    }
}
