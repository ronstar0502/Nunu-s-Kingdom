using System.Collections;
using UnityEngine;

public class Archer : CombatVillager
{
    [Header("Arrow Settings")]
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowSpawnTransform;

    [Header("Other Archer Settings")]
    public GuardTower assignedGuardTower;
    public bool isAssignedToGuardTower; //assigned archers cant get out of the tower , unassigned can go to combat 
    private int guardTowerSlot;
    private Vector2 targetTower;
    private bool isGoingToGuardTower;


    protected void Awake()
    {
        if(assignedGuardTower == null)
        {
            base.Awake();
        }
    }
    protected override void Start()
    {
        if(assignedGuardTower == null)
        {
            base.Start();
        }
    }
    protected void Update()
    {
        if(!isGoingToGuardTower)
        {
            base.Update();
        }
    }
    public override void ChangeToCombatMode() //changes the archer to combat mode if not in tower
    {
        if (!isAssignedToGuardTower)
        {
            SetState(AmiggaState.Combat);
        }
        else
        {
            print("cant be in combat since archer in tower");
        }
    }

    public override void ChangeToPatrolMode() //changes the archer back to patrol mode if not in tower
    {
        if (!isAssignedToGuardTower)
        {
            SetState(AmiggaState.Patrol);
        }
    }
    public void GoToAssignedGuardTower(GuardTower guardTower) // method to tell the archer to go to assigned tower
    {
        //print($"archer state 1: {villagerState}");
        SetState(AmiggaState.ProffesionAction);
        //print($"archer state 2: {villagerState}");

        assignedGuardTower = guardTower;
        isAssignedToGuardTower = true;
        guardTowerSlot = guardTower.GetAvailableSpotIndex();
        targetTower = new Vector2(guardTower.transform.position.x, transform.position.y);

        //print($"archer state 3: {villagerState}");
        SetState(AmiggaState.InProffesionBuilding);

        //print($"archer state 4: {villagerState}");
        StartCoroutine(ArcherGuardTowerArrival());

        attackRange += assignedGuardTower.GetAttackRangeBonus();
        //print($"archer in tower {gameObject.name} in slot {guardTowerSlot} has {attackRange} range");
    }
    private IEnumerator ArcherGuardTowerArrival() // used courtine to move the archer to the tower so it wont be in update all the time
    {
        isGoingToGuardTower = true;
        //print($"archer state 5: {villagerState}");
        while (assignedGuardTower != null)
        {
            AmiggaMoveToTarget(targetTower);
            //print($"archer state 6: {villagerState}");
            if (transform.position == (Vector3)targetTower)
            {
                //print($"archer state 7: {villagerState}");
                assignedGuardTower.AddArcherToGuard(gameObject, guardTowerSlot);
                //ChangeState(VillagerState.InProffesionBuilding);
                isGoingToGuardTower = false;
                yield break;
            }

            yield return null;
        }
    }

    protected override void SetAmmigarAttackTargetDirection()
    {
        if (targetEnemy != null)
        {
            SetAmmigaDirection(targetEnemy.transform.position.x);
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
            Vector2 targetDirection = targetEnemy.transform.position - gameObject.transform.position;

            if(targetDirection.magnitude < 0.1f) // if the distance of the arrow spawning from the enemy is very low
            {
                print($"Arrow spawned on the target {targetEnemy.name}, dealing {damage} damage");
                //deal the damage directly to the enemy
                targetEnemy.GetComponent<IDamageable>().TakeDamage(damage);
                return; //than no need to instatiate the arrow
            }

            //targetDirection.Normalize();

            GameObject arrowObj = Instantiate(arrowPrefab, arrowSpawnTransform.position, Quaternion.identity, arrowSpawnTransform);
            print(targetEnemy.name);
            arrowObj.GetComponent<Arrow>().InitArrow(targetEnemy, targetDirection, damage,isAssignedToGuardTower);
        }
        else
        {
            print($"target enemy is null");
        }
    }
}
