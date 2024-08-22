using UnityEngine;

public class ProffesionBuilding : Building
{
    [Header("Prefabs")]
    [SerializeField] protected GameObject villlagerProffesionPrefab;
    [Header("Transforms")]
    [SerializeField] protected Transform villagerRecruitSpot;
    [Header("Name")]
    [SerializeField] private string toolName;
    private HQ HQ;
    private Villager villager;

    private void Start()
    {
        HQ = FindObjectOfType<HQ>();
        villager = villlagerProffesionPrefab.GetComponent<Villager>();
    }

    public virtual void RecruitVillagerProffesion() //method for recruiting unemployed to the specific proffesion 
    {
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
        unemployedVillager.GoToProffesionBuilding(gameObject, villagerRecruitSpot.position);
        HQ.RemoveUnemployed(randomUnemployed);

    }
    public void VillagerProffesionChange_OnArrival(GameObject unemployedVillager)
    {
        Vector2 unemployedVillagerPosition = unemployedVillager.transform.position;
        Destroy(unemployedVillager);
        GameObject proffesionVillager = Instantiate(villlagerProffesionPrefab,unemployedVillagerPosition , Quaternion.identity);
        HQ.AddProffesionVillager(proffesionVillager);
    }
        
}
