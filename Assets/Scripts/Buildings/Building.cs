using UnityEngine;

public class Building : MonoBehaviour , IInteractable , IDamageable
{
    [SerializeField] protected BuildingData buildingData; //seperated the data to different script for easier use
    protected int nextLevelCost;
    private SpriteRenderer sr;
    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = buildingData.buildingSprites[0];
        buildingData.InitBuildingHP();        
    }
    private void Start()
    {
        print(buildingData.ToString());
        nextLevelCost = buildingData.GetNextLevelCost();
    }

    public BuildingData GetBuildingData() { return buildingData; }

    public void TakeDamage(int damage)
    {
        buildingData.TakeDamage(damage);
    }

    public void Interact() //for level up
    {
        UpgradeBuilding();
    }

    private void UpgradeBuilding()
    {
        if(player.GetPlayerData().seedAmount >= nextLevelCost)
        {
            LevelUpBuilding();
            print($"upgraded building: {buildingData.buildingName} to level: {buildingData.buildingLevel} and cost {nextLevelCost} seeds");
            nextLevelCost = buildingData.GetNextLevelCost();
        }
        else
        {
            print($"not enough seeds!! , you need {nextLevelCost} seeds and you have {player.GetPlayerData().seedAmount}");
        }
    }

    private void LevelUpBuilding()
    {
        player.GetPlayerData().SubstarctSeedsAmount(nextLevelCost);
        buildingData.LevelUp();
        sr.sprite = buildingData.buildingSprites[buildingData.buildingLevel - 1];
    }
}
