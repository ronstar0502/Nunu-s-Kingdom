using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HQ : Building
{
    //main building can only be one and cannot be constructed
    [SerializeField] private List<GameObject> unemployedVillagers;
    [SerializeField] private List<GameObject> proffesionVillagers;
    [SerializeField] private int maxVillagerAmount;
    [SerializeField] private int currentVillagerAmount;
    public void AddUnemployedVillager(GameObject villager)
    {
        unemployedVillagers.Add(villager);
        currentVillagerAmount++;
        print($"total villager {currentVillagerAmount} / {maxVillagerAmount} and {unemployedVillagers.Count} are unemployed");
    }

    public void AddProffesionVillager(GameObject villager)
    {
        proffesionVillagers.Add(villager);
        print($"total villager {currentVillagerAmount} / {maxVillagerAmount} and {proffesionVillagers.Count} are employed and {unemployedVillagers.Count} are unemployed");
    }

    public bool CanRecruitVillager()
    {
        return currentVillagerAmount < maxVillagerAmount;
    }

    public bool HasUnemployedVillager()
    {
        return unemployedVillagers.Count > 0;
    }

    public GameObject GetRandomUnemployed()
    {
        int randomIndex = Random.Range(0, unemployedVillagers.Count);
        return unemployedVillagers[randomIndex];
    }

    public void RemoveUnemployed(GameObject unemployed)
    {
        unemployedVillagers.Remove(unemployed);
        print($"now you have {unemployedVillagers.Count} unepmloyed villagers");
    }
}

