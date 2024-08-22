using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VillagerData
{
    public string villagerName;
    public int maxHealth;
    public int health;
    public float speed;
    public int seedsCost;

    public void InitHealth()
    {
        health = maxHealth;
    }
}
