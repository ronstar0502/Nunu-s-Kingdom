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

    public override void EnableBuildingPopUp()
    {

        buildingPopUp.SetActive(true);
        buildingPopUp.GetComponent<BuildingPopUp>().EnableBuildingPopUp(nextLevelCost, villager.GetAmiggaData().seedsCost);

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
        if (villager == null)
        {
            print("Villager data not found");
        }
        if (HQ.HasUnemployedVillager())
        {
            if (player.GetPlayerData().seedAmount >= villager.GetAmiggaData().seedsCost)
            {
                if (VillagerRecruitAction())
                {
                    player.GetPlayerData().SubstarctSeedsAmount(villager.GetAmiggaData().seedsCost);
                    HQ.villageInfoUI.SetSeedsText();
                }
            }
            else
            {
                print($"has unemployed villager but not enough seeds , you have {player.GetPlayerData().seedAmount} and you need {villager.GetAmiggaData().seedsCost}");
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
            StartCoroutine(unemployedVillager.ChangeAmiggaProffesion(gameObject, transform.position));
            return true;
        }
        return false;

    }
    public void VillagerProffesionChange_OnArrival(GameObject unemployedVillager) //destroying the unepmloyed and invoking for a delay of x amount of time to change proffesion
    {
        HQ.RemoveUnemployed(unemployedVillager);
        Invoke(nameof(ChangeVillagerProffesion),changeProffesionDelay);
        Destroy(unemployedVillager);
    }

    protected virtual void ChangeVillagerProffesion() // method to change unemployed to the building's proffesion
    {
        int randomSpawnPoint = Random.Range(0, villagerRecruitSpawnPoints.Length);
        GameObject proffesionVillager = Instantiate(villlagerProffesionPrefab, villagerRecruitSpawnPoints[randomSpawnPoint].position, Quaternion.identity);
        HQ.AddProffesionVillager(proffesionVillager, buildingData.buildingName);
    }
}
