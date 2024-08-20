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
        HQ =  FindObjectOfType<HQ>();
        maxEggHatching = buildingData.buildingLevel;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RecruitVillager();
        }
    }
    public void RecruitVillager()
    {
        if (HQ.CanRecruitVillager() && eggsHatching < maxEggHatching)
        {
            
            GameObject newEgg = Instantiate(eggPrefab,eggSpawnPoints[eggsHatching]);
            VillagerEgg villagerEgg = newEgg.GetComponent<VillagerEgg>();
            villagerEgg.InitEgg(villagerPrefab, villagerSpawnPoint);
            eggsHatching++;
        }
    }

    public void EggHatch()
    {
        eggsHatching--;
    }

    protected override void LevelUpBuilding()
    {
        base.LevelUpBuilding();
        maxEggHatching = buildingData.buildingLevel;
        print($"hatchery can hatch {maxEggHatching} now");
    }

}
