using UnityEngine;

public class Farm : ProffesionBuilding
{
    [Header("Farm Details")]
    [SerializeField] private int[] farmerSlots = new int[3];
    [SerializeField] private int idleFarmValue;
    [SerializeField] private int currentWorkingVillagers;
    private int _currentFarmerSlots;
    private int _harvestAmount;
    private bool _playerInRange;

    protected override void Start()
    {
        base.Start();
        HQ.AddFarm(gameObject.GetComponent<Farm>());

        _currentFarmerSlots = farmerSlots[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _playerInRange)
        {
            RecruitVillagerProffesion();
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

    public void FarmSeeds() //farm seeds
    {
        _harvestAmount = (idleFarmValue + currentWorkingVillagers) * buildingData.level;
        print($"harvest amount : {_harvestAmount}");
        if (player != null)
        {
            player.GetPlayerData().AddSeedsAmount(_harvestAmount);
            HQ.villageInfoUI.SetSeedsText();
        }
        else
        {
            print("player not found!!");
        }
    }

    public override void TakeDamage(int damage)
    {
        if (this != null)
        {
            buildingHealth -= damage;
            if (buildingHealth <= 0)
            {
                HQ.RemoveFarm(this);
                print($"{buildingData.buildingName} Destroyed!!");
                Destroy(gameObject);
            }
        }
    }

    public override void RecruitVillagerProffesion() //add farmer if there is an available slot 
    {
        if (!IsFarmFull())
        {
            base.RecruitVillagerProffesion();
        }
    }
    public bool IsFarmFull() //checks if there are no avalable slots
    {
        return currentWorkingVillagers == _currentFarmerSlots;
    }

    protected override void LevelUpBuilding()
    {
        base.LevelUpBuilding();
        _currentFarmerSlots = farmerSlots[buildingData.level - 1];
    }

    protected override void ChangeVillagerProffesion() //method to change unemployed to farmer
    {
        int randomSpawnPoint = Random.Range(0, villagerRecruitSpawnPoints.Length);
        GameObject proffesionVillager = Instantiate(villlagerProffesionPrefab, villagerRecruitSpawnPoints[randomSpawnPoint].position, Quaternion.identity, villagerRecruitSpawnPoints[randomSpawnPoint]);
        HQ.AddProffesionVillager(proffesionVillager, buildingData.buildingName);
    }



}
