using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQ : Building
{
    //main building can only be one and cannot be constructed
    [SerializeField] private int maxVillagerAmount;
    [SerializeField] private int currentVillagerAmount;

    [SerializeField] private List<GameObject> villagers;
    public void AddVillager(GameObject villager)
    {
        villagers.Add(villager);
        print(currentVillagerAmount);
    }

    public bool CanRecruitVillager()
    {
        return currentVillagerAmount < maxVillagerAmount;
    }
}

