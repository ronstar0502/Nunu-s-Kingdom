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

    public override void EnableBuildingPopUp()
    {
        buildingPopUp.SetActive(true);
        buildingPopUp.GetComponent<BuildingPopUp>().EnableBuildingPopUp(nextLevelCost,villager.GetVillagerData().seedsCost);
    }
    protected override void LevelUpBuilding()
    {
        base.LevelUpBuilding();
        HQ.villageInfoUI.SetSeedsText();
    }
    public virtual void RecruitVillagerProffesion() //method for recruiting unemployed to the specific proffesion 
    {
        if (HQ == null)
        {
            print("HQ is not found");
        }
        if(villager == null)
        {
            print("Villager data not found");
        }
        villager = villlagerProffesionPrefab.GetComponent<Villager>();
        if (HQ.HasUnemployedVillager())
        {
            if(player.GetPlayerData().seedAmount >= villager.GetVillagerData().seedsCost)
            {
                player.GetPlayerData().SubstarctSeedsAmount(villager.GetVillagerData().seedsCost);
                HQ.villageInfoUI.SetSeedsText();
                VillagerRecruitAction();
            }
            else
            {
                print($"has unemployed villager but not enough seeds , you have {player.GetPlayerData().seedAmount} and you need {villager.GetVillagerData().seedsCost}");
            }
        }
        else
        {
            print("no unemployed villager available, pls recruit more villager from the hatchery");
        }
    }
    private void VillagerRecruitAction() //method for the recruit an unemployed villager to a proffesion
    {
        GameObject randomUnemployed = HQ.GetRandomUnemployed();
        Villager unemployedVillager = randomUnemployed.GetComponent<Villager>();
        if(unemployedVillager != null)
        {
            unemployedVillager.ChangeVillagerProffesion(gameObject, transform.position);
            HQ.RemoveUnemployed(randomUnemployed);
        }

    }
    public void VillagerProffesionChange_OnArrival(GameObject unemployedVillager) //destroying the unepmloyed and invoking for a delay of x amount of time to change proffesion
    {
        Destroy(unemployedVillager);
        Invoke(nameof(ChangeVillagerProffesion), changeProffesionDelay);
    }

    protected virtual void ChangeVillagerProffesion() // method to change unemployed to the building's proffesion
    {
        int randomSpawnPoint = Random.Range(0, villagerRecruitSpawnPoints.Length);
        GameObject proffesionVillager = Instantiate(villlagerProffesionPrefab, villagerRecruitSpawnPoints[randomSpawnPoint].position, Quaternion.identity,transform);
        HQ.AddProffesionVillager(proffesionVillager,buildingData.buildingName);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        buildingSpot.SetActive(true);
    }
}
