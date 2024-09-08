using UnityEngine;

public class Hatchery : Building
{
    [Header("Prefabs")]
    [SerializeField] private GameObject unemployedVillagerPrefab;
    [SerializeField] private GameObject eggPrefab;
    [Header("Transforms")]
    [SerializeField] private Transform[] eggSpawnPoints = new Transform[3];
    [SerializeField] private Transform[] villagerSpawnPoints;
    private bool[] eggSlotsOpen = new bool[]{ true,false,false};
    [Header("Variables")]
    [SerializeField] private int villagerCost;
    private HQ HQ;   
    private bool playerInRange;

    private void Start()
    {
        HQ =  FindObjectOfType<HQ>(); //finds the hq game object for the hatchery to work
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&& playerInRange) //placeholder for testing
        {
            RecruitVillager();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    public void RecruitVillager() //recruits a villager
    {
        if (HQ.CanRecruitVillager() && HasOpenEggSlots() && player.GetPlayerData().seedAmount >= villagerCost) //checks if the player can recruit and there are available eggs slots
        {         
            HQ.AddToTotalVillagerAmount();
            player.GetPlayerData().SubstarctSeedsAmount(villagerCost);
            HQ.villageInfoUI.SetSeedsText();
            int eggSlot = GetEggSlotNumber();
            GameObject newEgg = Instantiate(eggPrefab,eggSpawnPoints[eggSlot]); //spawns an egg on pre determined transforms
            VillagerEgg villagerEgg = newEgg.GetComponent<VillagerEgg>();

            int randomSpawnPoint = Random.Range(0, villagerSpawnPoints.Length);
            villagerEgg.InitEgg(unemployedVillagerPrefab, villagerSpawnPoints[randomSpawnPoint], eggSlot); //sets the egg data and script
            eggSlotsOpen[eggSlot] = false;
        }
    }

    public void EggHatch(GameObject villager , int slotNumber)
    {
        HQ.AddUnemployedVillager(villager);
        eggSlotsOpen[slotNumber] = true;
    }

    protected override void LevelUpBuilding() //extension of level up building with more logic for hatchery
    {
        base.LevelUpBuilding();
        HQ.villageInfoUI.SetSeedsText();
        eggSlotsOpen[buildingData.level - 1] = true;
    }

    private bool HasOpenEggSlots()
    {
        for ( int i = 0; i < eggSlotsOpen.Length; i++ )
        {
            if (eggSlotsOpen[i])
            {
                return true;
            }
        }
        return false;
    }
    private int GetEggSlotNumber()
    {
        int slotIndex=0;
        for (int i=0;i<eggSlotsOpen.Length;i++)
        {
            if (eggSlotsOpen[i])
            {
                slotIndex = i;
                break;
            }
        }
        return slotIndex;
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        buildingSpot.SetActive(true);
    }
}
