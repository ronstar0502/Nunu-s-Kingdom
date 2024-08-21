using UnityEngine;

public class Building : MonoBehaviour, IInteractable, IDamageable
{
    [SerializeField] protected BuildingData buildingData; //seperated the data to different script for easier use
    protected int nextLevelCost;
    protected Player player;
    private SpriteRenderer sr;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = buildingData.buildingSprites[0];
        buildingData.InitBuildingHP();        
    }
    private void Start()
    {
        //print(buildingData.ToString());
        nextLevelCost = buildingData.GetNextLevelCost();
    }

    public BuildingData GetBuildingData() { return buildingData; }

    public void TakeDamage(int damage)
    {
        buildingData.TakeDamage(damage);
        if (buildingData.buildingHP <= 0)
        {
            DestroyBuilding();
        }
    }

    public void Interact() //interact for level up
    {
        UpgradeBuilding();
    }

    private void UpgradeBuilding() // method for upgrading a building when interacting with it
    {
        if(buildingData.buildingLevel < buildingData.maxBuildingLevel) //first check if the building is ont already in max level
        {
            if(player.GetPlayerData().seedAmount >= nextLevelCost) // checks if the player has enough seeds to upgrade
            {
                LevelUpBuilding();
                print($"upgraded building: {buildingData.buildingName} to level: {buildingData.buildingLevel} and cost {nextLevelCost} seeds");                
            }
            else
            {
                print($"not enough seeds!! , you need {nextLevelCost} seeds and you have {player.GetPlayerData().seedAmount}");
            }
            nextLevelCost = buildingData.GetNextLevelCost(); // after level up , sets the next level up cost
        }
    }

    protected virtual void LevelUpBuilding() //method for building level up
    {
        player.GetPlayerData().SubstarctSeedsAmount(nextLevelCost); //substracts seed from player based on level up cost
        buildingData.LevelUp(); //levels up building by adding 1 to the level
        sr.sprite = buildingData.buildingSprites[buildingData.buildingLevel - 1]; //changes the building visual based on level
    }
    private void DestroyBuilding()
    {
        //put building on destroy state
        if (this!=null)
        {
            Destroy(this.gameObject);
        }
    }
}
