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
    private int archersInGuard;

    private void Start()
    {
        HQ HQ = FindObjectOfType<HQ>();
        HQ.AddGuardTower(gameObject.GetComponent<GuardTower>());
    }

    public void AddArcherToGuard(GameObject archer)
    {
        archers.Add(archer);
        Transform spotTransform = archerSpots[GetAvailableSpotIndex()];
        archer.gameObject.transform.position = spotTransform.position;
        archer.gameObject.transform.SetParent(spotTransform);        
    }

    public bool hasOpenSlots()
    {
        for (int i = 0;i < buildingData.level; i++)
        {
            if (!availableSpots[i]) return false;
        }
        return true;
    }

    private int GetAvailableSpotIndex()
    {
        for(int i = 0; i < buildingData.level; i++)
        {
            if(availableSpots[i]) return i;
        }
        return 0;

    }

    public float GetAttackRangeBonus()
    {
        return attackRangeBonus;
    }
}
