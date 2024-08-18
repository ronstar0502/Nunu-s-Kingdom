using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archery : WeaponryBuilding
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            CraftWeapon();
        }
    }
}
