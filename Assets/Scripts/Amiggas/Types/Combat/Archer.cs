using System.Collections;
using UnityEngine;

public class Archer : CombatVillager
{
    public GuardTower assignedGuardTower;
    public Vector2 targetTower;
    public int guardTowerSlot;
    public bool isAssigned; //assigned archers cant get out of the tower , unassigned can go to combat 

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnTransform;

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
            SetState(VillagerState.Combat);
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
            SetState(VillagerState.Patrol);
        }
    }

    public void GoToAssignedGuardTower(GuardTower guardTower) // method to tell the archer to go to assigned tower
    {
        //print($"archer state 1: {villagerState}");
        SetState(VillagerState.ProffesionAction);
        //print($"archer state 2: {villagerState}");

        assignedGuardTower = guardTower;
        isAssigned = true;
        guardTowerSlot = guardTower.GetAvailableSpotIndex();
        targetTower = new Vector2(guardTower.transform.position.x, transform.position.y);

        //print($"archer state 3: {villagerState}");
        SetState(VillagerState.InProffesionBuilding);

        //print($"archer state 4: {villagerState}");
        StartCoroutine(ArcherGuardTowerArrival());

        attackRange += assignedGuardTower.GetAttackRangeBonus();
        //print($"archer in tower {gameObject.name} in slot {guardTowerSlot} has {attackRange} range");
    }

    /*private void CheckIfCanPatrol() //checks if archer can patrol
    {
        if (assignedGuardTower != null)
        {
            return;
        }
        if (villagerState == VillagerState.ProffesionAction || villagerState == VillagerState.InProffesionBuilding)
        {
            return;
        }
        SetState(VillagerState.Patrol);
    }*/
    private IEnumerator ArcherGuardTowerArrival() // used courtine to move the archer to the tower so it wont be in update all the time
    {
        //print($"archer state 5: {villagerState}");
        while (assignedGuardTower != null)
        {
            VillagerMoveToTarget(targetTower);
            //print($"archer state 6: {villagerState}");
            if (transform.position == (Vector3)targetTower)
            {
                //print($"archer state 7: {villagerState}");
                assignedGuardTower.AddArcherToGuard(gameObject, guardTowerSlot);
                //ChangeState(VillagerState.InProffesionBuilding);
                yield break;
            }

            yield return null;
        }
    }

    protected override void AttackTarget()
    {
        if (targetEnemy != null)
        {
            SetVillagerDirection(targetEnemy.transform.position.x);
            print("archer attacked!");
        }
        else
        {
            SetNewTarget();
        }
    }
    public void SpawnArrow()
    {
        if (targetEnemy != null)
        {
            GameObject arrowObj = Instantiate(arrowPrefab, arrowSpawnTransform.position, Quaternion.identity, arrowSpawnTransform);
            print(targetEnemy.name);
            Vector2 direction = targetEnemy.transform.position - gameObject.transform.position;
            arrowObj.GetComponent<Arrow>().InitArrow(targetEnemy, direction, damage);
        }
        else
        {
            print($"target enemy is null");
        }
    }
}
