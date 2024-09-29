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
    private int _guardTowerSlot;
    private Vector2 _targetTower;
    private bool _isGoingToGuardTower;

    protected override void Awake()
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

    protected override void Update()
    {
        if(!_isGoingToGuardTower)
        {
            base.Update();
        }
    }

    public override void ChangeToCombatMode() //changes the archer to combat mode if not in tower
    {
        if (!isAssignedToGuardTower)
        {
            SetState(VillagerState.Combat);
        }
        else
        {
            print("cant be in combat since archer in tower");
        }
    }

    public override void ChangeToDayMode() //changes the archer back to patrol mode if not in tower
    {
        if (!isAssignedToGuardTower)
        {
            SetState(VillagerState.Patrol);
        }
    }

    public void GoToAssignedGuardTower(GuardTower guardTower) // method to tell the archer to go to assigned tower
    {
        SetState(VillagerState.ProffesionAction);

        assignedGuardTower = guardTower;
        isAssignedToGuardTower = true;
        _guardTowerSlot = guardTower.GetAvailableSpotIndex();
        _targetTower = new Vector2(guardTower.transform.position.x, transform.position.y);

        SetState(VillagerState.InProffesionBuilding);

        StartCoroutine(MoveARcherToAssignedGuardTower());

        attackRange += assignedGuardTower.GetAttackRangeBonus();
    }
    private IEnumerator MoveARcherToAssignedGuardTower() // used courtine to move the archer to the tower so it wont be in update all the time
    {
        _isGoingToGuardTower = true;
        while (assignedGuardTower != null)
        {
            VillagerMoveToTarget(_targetTower);
            if (transform.position == (Vector3)_targetTower)
            {
                assignedGuardTower.AddArcherToGuard(gameObject, _guardTowerSlot);
                _isGoingToGuardTower = false;
                yield break;
            }

            yield return null;
        }
    }

    protected override void SetVillagerAttackTargetDirection()
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
            Vector2 targetDirection = targetEnemy.transform.position - gameObject.transform.position;

            if(targetDirection.magnitude < 0.2f) // if the distance of the arrow spawning from the enemy is very low
            {
                print($"Arrow spawned on the target {targetEnemy.name}, dealing {damage} damage");
                //deal the damage directly to the enemy
                targetEnemy.GetComponent<IDamageable>().TakeDamage(damage);
                return; //than no need to instatiate the arrow
            }

            //targetDirection.Normalize();

            GameObject arrowObj = Instantiate(arrowPrefab, transform.position, Quaternion.identity, arrowSpawnTransform);
            print(targetEnemy.name);
            arrowObj.GetComponent<Arrow>().InitArrow(targetEnemy, targetDirection, damage,isAssignedToGuardTower);
        }
        else
        {
            print($"target enemy is null");
        }
    }
}
