using UnityEngine;

public class Hatchery : Building
{
    [Header("Prefabs")]
    [SerializeField] private GameObject unemployedVillagerPrefab;
    [SerializeField] private GameObject eggPrefab;
    [Header("Transforms")]
    [SerializeField] private Transform[] eggSpawnPoints = new Transform[3];
    [SerializeField] private Transform[] villagerSpawnPoints;
    private bool[] _eggSlotsOpen = new bool[] { true, false, false };
    private int _villagerCost;
    private HQ _HQ;
    private bool _playerInRange;

    protected void Start()
    {
        _HQ = FindObjectOfType<HQ>(); //finds the hq game object for the hatchery to work
        _villagerCost = unemployedVillagerPrefab.GetComponent<Villager>().GetVillagerData().seedsCost;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _playerInRange) //placeholder for testing
        {
            RecruitVillager();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }

    protected override void RefreshPopUp()
    {
        // for hatchery building that can recruit
        buildingPopUp.GetComponent<BuildingPopUp>().EnableBuildingPopUp(nextLevelCost, _villagerCost,CanRecruitVillager(),buildingData.level);
    }

    public void RecruitVillager() //recruits a villager
    {
        if (CanRecruitVillager()) //checks if the player can recruit and there are available eggs slots
        {
            _HQ.AddToTotalVillagerAmount();
            player.GetPlayerData().SubstarctSeedsAmount(_villagerCost);
            villageInfoUI.SetSeedsText(true);
            int eggSlot = GetEggSlotNumber();
            GameObject newEgg = Instantiate(eggPrefab, eggSpawnPoints[eggSlot]); //spawns an egg on pre determined transforms
            VillagerEgg villagerEgg = newEgg.GetComponent<VillagerEgg>();

            int randomSpawnPoint = Random.Range(0, villagerSpawnPoints.Length);
            villagerEgg.InitEgg(unemployedVillagerPrefab, villagerSpawnPoints[randomSpawnPoint], eggSlot); //sets the egg data and script
            _eggSlotsOpen[eggSlot] = false;
            InvokeBuildingStateChanged();
        }
    }

    public bool CanRecruitVillager()
    {
        return _HQ.CanRecruitVillager() && HasOpenEggSlots() && player.GetPlayerData().seedAmount >= _villagerCost;
    }

    public void EggHatch(GameObject amigga, int slotNumber)
    {
        _HQ.AddUnemployedVillager(amigga);
        _eggSlotsOpen[slotNumber] = true;
        RefreshPopUp();
    }

    protected override void LevelUpBuilding() //extension of level up building with more logic for hatchery
    {
        base.LevelUpBuilding();
        villageInfoUI.SetSeedsText(true);
        _eggSlotsOpen[buildingData.level - 1] = true;
    }

    private bool HasOpenEggSlots()
    {
        for (int i = 0; i < _eggSlotsOpen.Length; i++)
        {
            if (_eggSlotsOpen[i])
            {
                return true;
            }
        }
        return false;
    }

    private int GetEggSlotNumber()
    {
        int slotIndex = 0;
        for (int i = 0; i < _eggSlotsOpen.Length; i++)
        {
            if (_eggSlotsOpen[i])
            {
                slotIndex = i;
                break;
            }
        }
        return slotIndex;
    }
}
