using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VillagerData
{
    [SerializeField] protected string villagerName;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int health;
    [SerializeField] protected float speed;

    [SerializeField] protected int productionCost;

    public void InitHealth()
    {
        health = maxHealth;
    }
}
