using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardTower : Building
{
    [Header("Archer Spots")]
    [SerializeField] private Transform[] archerSpots = new Transform[3];
    private bool[] availableSpots = new bool[] { true, false, false };
    [Header("Archers Data")]
    [SerializeField] private List<GameObject> archers;
    [SerializeField] private float attackRangeBonus;
    private HQ HQ;

    protected void Start()
    {
        HQ = FindObjectOfType<HQ>();

        HQ.AddGuardTower(gameObject.GetComponent<GuardTower>());
        CanAssignArcher();

    }
    public void CanAssignArcher() //checks if can assign an archer to guard tower
    {
        if (hasOpenSlots() && HQ.ArcherCount()>0)
        {
            GameObject archerToassign = HQ.GetRandomArcher();
            if (archerToassign != null)
            {
                archerToassign.gameObject.GetComponent<Archer>().GoToAssignedGuardTower(gameObject.GetComponent<GuardTower>());
            }
            else
            {
                print("no archers found");
            }
        }
    }

    public void AddArcherToGuard(GameObject archer,int slot) //method to add the assigned archer to the guard tower
    {
        archers.Add(archer);
        //print($"slot index for archer placement {slot}");
        Transform spotTransform = archerSpots[slot];
        archer.gameObject.transform.position = spotTransform.position;
        archer.gameObject.transform.SetParent(spotTransform);
    }

    public bool hasOpenSlots() //checks if the guard tower has open slots to assign an archer to
    {
        for (int i = 0;i < buildingData.level; i++)
        {
            if (availableSpots[i]) 
            {
                //print("guard tower has open slots");
                return true; 
            }
        }
        return false;

    }

    public int GetAvailableSpotIndex() //gets available slot in the guard tower
    {
        for(int i = 0; i < buildingData.level; i++)
        {
            if (availableSpots[i])
            {
                availableSpots[i] = false;
                return i;
            }
        }
        return -1; //if there are no slots open

    }

    public float GetAttackRangeBonus()
    {
        return attackRangeBonus;
    }

    protected override void LevelUpBuilding()
    {
        base.LevelUpBuilding();
        availableSpots[buildingData.level-1] = true;

        CanAssignArcher();
    }

    public override void TakeDamage(int damage)
    {
        if (this != null) // checks if this building is not destroyed
        {
            buildingHealth -= damage;
            //print($"{buildingData.buildingName} took {damage} damage and now has {buildingHealth}!!");
            if (buildingHealth <= 0)
            {
                print($"{buildingData.buildingName} Destroyed!!");
                DestroyArcherInTower();
                DestroyBuilding();
            }
        }
        buildingSpot.SetActive(true);
    }

    private void DestroyArcherInTower()
    {
        for(int i = 0; i < archers.Count; i++)
        {
            Destroy(archers[i]);
        }
    }
}
