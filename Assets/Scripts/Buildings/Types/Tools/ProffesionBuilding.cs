using UnityEngine;

public class ProffesionBuilding : Building
{
    [Header("Prefabs")]
    [SerializeField] protected GameObject villlagerProffesionPrefab;
    [Header("Transforms")]
    [SerializeField] protected Transform villagerRecruitSpot;
    [SerializeField] private Transform[] villagerRecruitSpawnPoints;
    [Header("Other Data")]
    [SerializeField] private string toolName;
    [SerializeField] private float changeProffesionDelay;
    private HQ HQ;
    private Villager villager;

    private void Start()
    {
        HQ = FindObjectOfType<HQ>();
        villager = villlagerProffesionPrefab.GetComponent<Villager>();
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
    private void VillagerRecruitAction()
    {
        GameObject randomUnemployed = HQ.GetRandomUnemployed();
        Villager unemployedVillager = randomUnemployed.GetComponent<Villager>();
        unemployedVillager.ChangeProffesion(gameObject, transform.position);
        HQ.RemoveUnemployed(randomUnemployed);

    }
    public void VillagerProffesionChange_OnArrival(GameObject unemployedVillager) //destroying the unepmloyed and invoking for a delay of x amount of time to change proffesion
    {
        Destroy(unemployedVillager);
        Invoke(nameof(ChangeVillagerProffesion), changeProffesionDelay);
    }

    private void ChangeVillagerProffesion()
    {
        int randomSpawnPoint = Random.Range(0, villagerRecruitSpawnPoints.Length);
        GameObject proffesionVillager = Instantiate(villlagerProffesionPrefab, villagerRecruitSpawnPoints[randomSpawnPoint].position, Quaternion.identity);
        HQ.AddProffesionVillager(proffesionVillager,buildingData.buildingName);
    }
}
