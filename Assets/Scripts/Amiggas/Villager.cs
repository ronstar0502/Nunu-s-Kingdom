using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : MonoBehaviour
{
    [SerializeField] protected VillagerData villagerData;

    private void Awake()
    {
        villagerData.InitHealth();
    }
}
