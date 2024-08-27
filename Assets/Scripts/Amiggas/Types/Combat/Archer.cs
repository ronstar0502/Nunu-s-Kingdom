using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Archer : CombatVillager
{
    private GuardTower assignedGuardTower;
    private Vector2 targetTower;
    private bool isAssigned;
    private void Update()
    {
        if (!isAssigned && assignedGuardTower != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetTower, villagerData.speed * Time.deltaTime);
            if (transform.position == (Vector3)targetTower)
            {
                isAssigned = true;
                assignedGuardTower.AddArcherToGuard(gameObject);
            }
        }
    }
    public void GoToGuardTower(GuardTower guardTower)
    {
        assignedGuardTower = guardTower;
        targetTower = new Vector2(guardTower.transform.position.x, transform.position.y);
    }
}
