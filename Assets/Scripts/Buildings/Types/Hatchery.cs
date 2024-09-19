using UnityEngine;

public class Hatchery : Building
{
    [Header("Prefabs")]
    [SerializeField] private GameObject unemployedAmiggaPrefab;
    [SerializeField] private GameObject eggPrefab;
    [Header("Transforms")]
    [SerializeField] private Transform[] eggSpawnPoints = new Transform[3];
    [SerializeField] private Transform[] amiggaSpawnPoints;
    private bool[] eggSlotsOpen = new bool[] { true, false, false };
    [Header("Variables")]
    [SerializeField] private int amiggaCost;
    private HQ HQ;
    private bool playerInRange;

    protected void Start()
    {
        HQ = FindObjectOfType<HQ>(); //finds the hq game object for the hatchery to work
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange) //placeholder for testing
        {
            RecruitAmmiga();
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
    public override void EnableBuildingPopUp()
    {
        buildingPopUp.SetActive(true);
        buildingPopUp.GetComponent<BuildingPopUp>().EnableBuildingPopUp(nextLevelCost, amiggaCost);
    }
    public void RecruitAmmiga() //recruits a villager
    {
        if (CanRecruitAmmiga()) //checks if the player can recruit and there are available eggs slots
        {
            HQ.AddToTotalAmmigaAmount();
            player.GetPlayerData().SubstarctSeedsAmount(amiggaCost);
            HQ.villageInfoUI.SetSeedsText();
            int eggSlot = GetEggSlotNumber();
            GameObject newEgg = Instantiate(eggPrefab, eggSpawnPoints[eggSlot]); //spawns an egg on pre determined transforms
            VillagerEgg amiggaEgg = newEgg.GetComponent<VillagerEgg>();

            int randomSpawnPoint = Random.Range(0, amiggaSpawnPoints.Length);
            amiggaEgg.InitEgg(unemployedAmiggaPrefab, amiggaSpawnPoints[randomSpawnPoint], eggSlot); //sets the egg data and script
            eggSlotsOpen[eggSlot] = false;
        }
    }

    public bool CanRecruitAmmiga()
    {
        return HQ.CanRecruitVillager() && HasOpenEggSlots() && player.GetPlayerData().seedAmount >= amiggaCost;
    }
    public void EggHatch(GameObject amigga, int slotNumber)
    {
        HQ.AddUnemployedVillager(amigga);
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
        for (int i = 0; i < eggSlotsOpen.Length; i++)
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
        int slotIndex = 0;
        for (int i = 0; i < eggSlotsOpen.Length; i++)
        {
            if (eggSlotsOpen[i])
            {
                slotIndex = i;
                break;
            }
        }
        return slotIndex;
    }
}
