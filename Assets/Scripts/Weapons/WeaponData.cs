using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    [SerializeField] protected Sprite sprite;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected int cost;
}
