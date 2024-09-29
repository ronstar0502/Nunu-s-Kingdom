using System;
using UnityEngine;

public class Building : MonoBehaviour, IInteractable, IDamageable
{
    [SerializeField] protected BuildingData buildingData;
    [SerializeField] protected GameObject buildingSpot;
    [SerializeField] protected GameObject buildingPopUp;
    [SerializeField] protected event Action OnBuildingStateChanged;
    [SerializeField] protected float buildingHealth;
    protected Player player;
    protected int nextLevelCost;
    protected bool IsMaxLevel;
    private SpriteRenderer _sr;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        _sr = GetComponent<SpriteRenderer>();
        _sr.sprite = buildingData.sprite;
        buildingHealth = buildingData.health;
        SetNextLevelCost();
    }

    public BuildingData GetBuildingData() { return buildingData; }

    public void SetBuildingSpot(GameObject buildSpot)
    {
        buildingSpot = buildSpot;
    }

    public virtual void TakeDamage(int damage)
    {
        if (this != null) // checks if this building is not destroyed
        {
            buildingHealth -= damage;
            //print($"{buildingData.buildingName} took {damage} damage and now has {buildingHealth}!!");
            if (buildingHealth <= 0)
            {
                print($"{buildingData.buildingName} Destroyed!!");
                DestroyBuilding();
            }
        }
    }

    public void EnableBuildingPopUp()
    {
        buildingPopUp.SetActive(true);
        OnBuildingStateChanged += RefreshPopUp;
        RefreshPopUp();
    }

    public void DisableBuildingPopUp()
    {
        buildingPopUp.SetActive(false);
        OnBuildingStateChanged -= RefreshPopUp;
    }

    protected virtual void RefreshPopUp()
    {
        // default for buildings that can only be upgraded
        if (!buildingData.isMaxLevel)
        {
            buildingPopUp.GetComponent<BuildingPopUp>().EnableBuildingPopUp(nextLevelCost,buildingData.level);
        }
        else
        {
            buildingPopUp.GetComponent<BuildingPopUp>().SetToMaxLevelBuildingPopUp();
        }
    }

    protected void InvokeBuildingStateChanged()
    {
        OnBuildingStateChanged?.Invoke();
    }

    public void Interact() //interact for level up
    {

        UpgradeBuilding();

    }

    private void UpgradeBuilding() // method for upgrading a building when interacting with it
    {
        if (!buildingData.isMaxLevel) //first check if the building is ont already in max level
        {
            if (player.GetPlayerData().seedAmount >= nextLevelCost) // checks if the player has enough seeds to upgrade
            {
                LevelUpBuilding();
                print($"upgraded building: {buildingData.buildingName} to level: {buildingData.level} and cost {nextLevelCost} seeds next and has {buildingHealth} health points now");
            }
            else
            {
                print($"not enough seeds!! , you need {nextLevelCost} seeds and you have {player.GetPlayerData().seedAmount}");
            }
            SetNextLevelCost(); // after level up , sets the next level up cost
            OnBuildingStateChanged?.Invoke();
        }
    }

    protected virtual void LevelUpBuilding() //method for building level up
    {
        player.GetPlayerData().SubstarctSeedsAmount(nextLevelCost); //substracts seed from player based on level up cost
        buildingData = buildingData.nextLevelBuilding; //levels up building by adding 1 to the level
        //after that now we set the new level data
        _sr.sprite = buildingData.sprite;
        buildingHealth = buildingData.health;
    }

    private void SetNextLevelCost()
    {
        if (!buildingData.isMaxLevel)
        {
            nextLevelCost = buildingData.nextLevelBuilding.cost;
        }
        else
        {
            IsMaxLevel = true;
        }
    }

    protected void DestroyBuilding()
    {
        //put building on destroy state
        if (this != null)
        {
            if (buildingSpot != null)
            {
                buildingSpot.SetActive(true);
            }
            Destroy(gameObject);
        }
    }

}
