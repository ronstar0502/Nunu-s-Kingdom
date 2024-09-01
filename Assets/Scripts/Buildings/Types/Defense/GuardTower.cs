using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardTower : Building
{
    //Guard Tower
    //make slots for archers depends on level
    //only when an archer is in the guard tower only than he can shoot
    //make the archer on the guard tower shoot at a direction and the first enemy to get hit the arrow will deal damage to
    [Header("Archer Spots")]
    [SerializeField] private Transform[] archerSpots = new Transform[3];
    private bool[] availableSpots = new bool[] { true, false, false };
    [Header("Archers Data")]
    [SerializeField] private List<GameObject> archers;
    [SerializeField] private float attackRangeBonus;

    private HQ HQ;
    private int archersInGuard;

    private void Start()
    {
        HQ = FindObjectOfType<HQ>();
        HQ.AddGuardTower(gameObject.GetComponent<GuardTower>());
        
        CanAssignArcher();
    }

    public void CanAssignArcher()
    {
        if (hasOpenSlots() && HQ.ArcherCount()>0)
        {
            GameObject archerToassign = HQ.GetRandomArcher();
            if (archerToassign != null)
            {
                archerToassign.gameObject.GetComponent<Archer>().GoToGuardTower(gameObject.GetComponent<GuardTower>());
            }
            else
            {
                print("no archers found");
            }
        }
    }

    public void AddArcherToGuard(GameObject archer,int slot)
    {
        archers.Add(archer);
        print($"slot index for archer placement {slot}");
        Transform spotTransform = archerSpots[slot];
        archer.gameObject.transform.position = spotTransform.position;
        archer.gameObject.transform.SetParent(spotTransform);
        //availableSpots[slot] = false;
    }

    public bool hasOpenSlots()
    {
        for (int i = 0;i < buildingData.level; i++)
        {
            if (availableSpots[i]) 
            {
                print("guard tower has open slots");
                return true; 
            }
        }
        return false;

    }

    public int GetAvailableSpotIndex()
    {
        for(int i = 0; i < buildingData.level; i++)
        {
            if (availableSpots[i])
            {
                print($"slot index: {i} -> available? {availableSpots[i]}");
                availableSpots[i] = false;
                print($"slot index: {i} -> available? {availableSpots[i]}");
                return i;
            }
        }
        return -1;

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
}
