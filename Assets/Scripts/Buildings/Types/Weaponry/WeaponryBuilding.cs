using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponryBuilding : Building
{
    [Header("Prefabs")]
    [SerializeField] protected GameObject weaponPrefab;
    [Header("Visual Transforms")]
    [SerializeField] private Transform[] weaponVisualTransforms = new Transform[3];
    [Header("Name")]
    [SerializeField] private string weaponName;
    protected int weaponAmount;

    protected void CraftWeapon()
    {
        weaponAmount++;
        UpdateVisualTransforms();
        print($"{weaponName} in BlackSmith: {weaponAmount}");
    }

    private void UpdateVisualTransforms()
    {
        if(weaponAmount == 1)
        {
            Instantiate(weaponPrefab, weaponVisualTransforms[0]);
        }else if(weaponAmount == 5)
        {
            Instantiate(weaponPrefab, weaponVisualTransforms[1]);
        }
        else if(weaponAmount == 12)
        {
            Instantiate(weaponPrefab, weaponVisualTransforms[2]);
        }

    }
}
