using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour
{
    [SerializeField] protected VillagerData villagerData;
    [SerializeField] protected GameObject villagerTool; // unemployed doesnt have a tool

    private void Awake()
    {
        villagerData.InitHealth();
    }
}
