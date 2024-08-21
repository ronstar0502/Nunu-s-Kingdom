using UnityEngine;

public class Hatchery : Building
{
    [Header("Prefabs")]
    [SerializeField] private GameObject villagerPrefab;
    [SerializeField] private GameObject eggPrefab;
    [Header("Transforms")]
    [SerializeField] private Transform[] eggSpawnPoints = new Transform[3];
    [SerializeField] private Transform villagerSpawnPoint;
    [Header("Variables")]
    [SerializeField] private int villagerCost;
    private HQ HQ;
    private int maxEggHatching;
    private int eggsHatching;

    private void Start()
    {
        HQ =  FindObjectOfType<HQ>(); //finds the hq game object for the hatchery to work
        maxEggHatching = buildingData.buildingLevel;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) //placeholder for testing
        {
            RecruitVillager();
        }
    }
    public void RecruitVillager() //recruits a villager
    {
        if (HQ.CanRecruitVillager() && eggsHatching < maxEggHatching) //checks if the player can recruit and there are available eggs slots
        {
            
            GameObject newEgg = Instantiate(eggPrefab,eggSpawnPoints[eggsHatching]); //spawns an egg on pre determined transforms
            VillagerEgg villagerEgg = newEgg.GetComponent<VillagerEgg>();
            villagerEgg.InitEgg(villagerPrefab, villagerSpawnPoint); //sets the egg data and script
            eggsHatching++;
        }
    }

    public void EggHatch()
    {
        eggsHatching--;
    }

    protected override void LevelUpBuilding() //extension of level up building with more logic for hatchery
    {
        base.LevelUpBuilding();
        maxEggHatching = buildingData.buildingLevel;
        print($"hatchery can hatch {maxEggHatching} now");
    }

}
