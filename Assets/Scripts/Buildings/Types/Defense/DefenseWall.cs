using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseWall : Building
{
    //TBD

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        buildingSpot.SetActive(true);
    }
}
