using System.Collections;
using UnityEngine;

public class Archer : CombatVillager
{
    public GuardTower assignedGuardTower;
    public Vector2 targetTower;
    public int guardTowerSlot;
    public bool isAssigned; //assigned archers cant get out of the tower , unassigned can go to combat 

    protected override void Start()
    {
        if(assignedGuardTower == null)
        {
            base.Start();
        }
    }
    public override void ChangeToCombatMode() //changes the archer to combat mode if not in tower
    {
        if (!isAssigned)
        {
            ChangeState(VillagerState.Combat);
        }
        else
        {
            print("cant be in combat since archer in tower");
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
        print($"archer state 1: {villagerState}");
        ChangeState(VillagerState.ProffesionAction);
        print($"archer state 2: {villagerState}");

        assignedGuardTower = guardTower;
        guardTowerSlot = guardTower.GetAvailableSpotIndex();
        targetTower = new Vector2(guardTower.transform.position.x, transform.position.y);

        print($"archer state 3: {villagerState}");
        ChangeState(VillagerState.InProffesionBuilding);

        print($"archer state 4: {villagerState}");
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
        print($"archer state 5: {villagerState}");
        while (!isAssigned && assignedGuardTower != null)
        {
            VillagerMoveTo(targetTower);
            print($"archer state 6: {villagerState}");
            if (transform.position == (Vector3)targetTower)
            {
                print($"archer state 7: {villagerState}");
                isAssigned = true;
                assignedGuardTower.AddArcherToGuard(gameObject, guardTowerSlot);
                //ChangeState(VillagerState.InProffesionBuilding);
                yield break;
            }

            yield return null;
        }
    }
}
