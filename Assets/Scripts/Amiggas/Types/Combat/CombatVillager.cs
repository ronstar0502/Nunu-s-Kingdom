using UnityEngine;

public class CombatVillager : Villager
{
    [Header("Combat Unit Data")]
    [SerializeField] protected int damage;
    [SerializeField] protected float targetDetectRange;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackSpeed;
    protected GameObject targetEnemy;

    protected override void Awake()
    {
        base.Awake();
        if (HQ.isNightMode) //has some bugs with new recruited ammigas when spawned at night
        {
            SetState(VillagerState.Combat);
            SetNewTarget();
            if (targetEnemy != null)
            {
                print($"{villagerData.villagerName} is spawned at night and switched to combat mode | {villagerState} and has target: {targetEnemy.name}");
            }
            else
            {
                print($"{villagerData.villagerName} is spawned at night and switched to combat mode | {villagerState} but has no target.");
            }
        }
        
    }
    protected override void Start()
    {
        if (!HQ.isNightMode)
        {
            base.Start();
        }
    }

    protected override void Update()
    {
        base.Update();
        if (villagerState == VillagerState.Combat || villagerState == VillagerState.InProffesionBuilding)
        {
            print($"{villagerData.villagerName} is in combat mode.");
            if (targetEnemy == null || !targetEnemy.activeInHierarchy) //if target is not in hierarchy and null sets new target
            {
                print($"{villagerData.villagerName} has no target or target is inactive. Setting new target.");
                SetNewTarget();
            }

            if (targetEnemy != null)
            {
                print($"{villagerData.villagerName} found a target: {targetEnemy.name}.");
                SetVillagerAttackTargetDirection();
                if (IsInAttackRange())
                {
                    animator.SetBool("isAttacking",true);
                    print($"{villagerData.villagerName} is in attack range of {targetEnemy.name}. Attacking...");
                }
                else
                {
                    if (villagerState == VillagerState.Combat)
                    {
                        print($"{villagerData.villagerName} is moving towards {targetEnemy.name}.");
                        animator.SetBool("isAttacking",false);
                        VillagerMoveToTarget(targetEnemy.transform.position);
                    }
                }
            }
            else if (villagerState == VillagerState.Combat)
            {
                print($"{villagerData.villagerName} has no target and is now patrolling.");
                animator.SetBool("isAttacking", false);
                VillagerPatrol();
            }
        }
    }

    public void AttackTarget()
    {
        if (targetEnemy != null)
        {
            targetEnemy.GetComponent<IDamageable>().TakeDamage(damage);
            print($"{villagerData.villagerName} attacked {targetEnemy.name} with {damage} damage.");
        }
    }

    public virtual void ChangeToCombatMode() // change the combat villager to combat mode
    {
        SetState(VillagerState.Combat);
    }

    public virtual void ChangeToDayMode() // change the combat villager back to patrol mode
    {
        if (targetEnemy == null && !HQ.isNightMode)
        {
            SetState(VillagerState.Patrol);
            animator.SetBool("isAttacking", false);
        }
    }

    protected void SetNewTarget() //method to set new target
    {
        print($"{villagerData.villagerName} is searching for new targets within range {targetDetectRange}.");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, targetDetectRange);

        if (colliders.Length == 0)
        {
            print($"{villagerData.villagerName} found no enemies within range.");
            targetEnemy = null;
            if (!HQ.isNightMode)
            {
                SetState(VillagerState.Patrol);
            }
            return;
        }

        float minDistance = float.MaxValue;
        GameObject closestEnemy = null;

        for (int i = 0; i < colliders.Length; i++)
        {
            Enemy enemy = colliders[i].gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                Vector2 combatAmmigaPos = new Vector2(transform.position.x,0); // the 2 variable below are to check the distance on the x axsis only
                Vector2 enemyPos = new Vector2(enemy.transform.position.x,0);
                float distance = Vector2.Distance(combatAmmigaPos,enemyPos);                
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy.gameObject;
                }
            }
        }
        if (closestEnemy != null)
        {
            targetEnemy = closestEnemy;
            print($"{villagerData.villagerName} found a new target: {targetEnemy.name}.");
        }
        else
        {
            targetEnemy = null;
            print($"{villagerData.villagerName} did not find any valid enemies.");
        }
    }

    protected virtual void SetVillagerAttackTargetDirection() //method to attack the target by using the target's interface
    {
        if (targetEnemy != null)
        {
            SetVillagerDirection(targetEnemy.transform.position.x);
        }
        else
        {
            SetNewTarget();
        }
    }

    private bool IsInAttackRange() //checks if villager in attack range
    {
        if (targetEnemy == null) return false;
        float distance = Mathf.Abs(transform.position.x - targetEnemy.transform.position.x); //with vector2.Distance() had some troubles with the target set and attack range
        return distance <= attackRange + 0.1f;
    }
}
