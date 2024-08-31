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

    private void CheckIfCanPatrol()
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
    public void GoToGuardTower(GuardTower guardTower)
    {
        ChangeState(VillagerState.ProffesionAction);
        assignedGuardTower = guardTower;
        guardTowerSlot = guardTower.GetAvailableSpotIndex();
        targetTower = new Vector2(guardTower.transform.position.x, transform.position.y);
        ChangeState(VillagerState.InProffesionBuilding);
        StartCoroutine(ArcherTowerArrival());
    }

    private IEnumerator ArcherTowerArrival()
    {
        while (!isAssigned && assignedGuardTower != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetTower, villagerData.speed * Time.deltaTime);
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
