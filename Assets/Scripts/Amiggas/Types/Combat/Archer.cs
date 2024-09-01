using System.Collections;
using UnityEngine;

public class Archer : CombatVillager
{
    private GuardTower assignedGuardTower;
    private Vector2 targetTower;
    private int guardTowerSlot;
    public bool isAssigned; //assigned archers cant get out of the tower , unassigned can go to combat 

    protected override void Start()
    {
        InitVillager();
        Invoke(nameof(CheckIfCanPatrol),2f);
    }
    public override void ChangeToCombatMode() //changes the archer to combat mode if not in tower
    {
        if (!isAssigned)
        {
            ChangeState(VillagerState.Combat);
        }
    }

    public override void ChangeToPatrolMode() //changes the archer back to patrol mode if not in tower
    {
        if (!isAssigned)
        {
            ChangeState(VillagerState.Patrol);
        }
    }

    public void GoToGuardTower(GuardTower guardTower) // method to tell the archer to go to assigned tower
    {
        ChangeState(VillagerState.ProffesionAction);

        assignedGuardTower = guardTower;
        guardTowerSlot = guardTower.GetAvailableSpotIndex();
        targetTower = new Vector2(guardTower.transform.position.x, transform.position.y);

        ChangeState(VillagerState.InProffesionBuilding);
        StartCoroutine(ArcherTowerArrival());

        attackRange += assignedGuardTower.GetAttackRangeBonus();
        print($"archer in tower {gameObject.name} in slot {guardTowerSlot} has {attackRange} range");
    }

    private void CheckIfCanPatrol() //checks if archer can patrol
    {
        if (assignedGuardTower != null)
        {
            return;
        }
        if (villagerState == VillagerState.ProffesionAction || villagerState == VillagerState.InProffesionBuilding)
        {
            return;
        }
        ChangeState(VillagerState.Patrol);
    }
    private IEnumerator ArcherTowerArrival() // used courtine to move the archer to the tower so it wont be in update all the time
    {
        while (!isAssigned && assignedGuardTower != null)
        {
            //transform.position = Vector2.MoveTowards(transform.position, targetTower, villagerData.movementSpeed * Time.deltaTime);
            VillagerMoveTo(targetTower);
            if (transform.position == (Vector3)targetTower)
            {
                isAssigned = true;
                assignedGuardTower.AddArcherToGuard(gameObject, guardTowerSlot);
                //ChangeState(VillagerState.InProffesionBuilding);
                yield break;
            }

            yield return null;
        }
    }
}
