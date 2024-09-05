using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatVillager : Villager
{
    [Header("Combat Unit Data")]
    [SerializeField] protected int damage;
    [SerializeField] protected float targetDetectRange;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float attackSpeed;
    protected GameObject targetEnemy;
    private float attackTimer;

    protected override void Start()
    {
        attackTimer = attackSpeed;
        if (HQ.isNightMode) //has some bugs with new recruited ammigas when spawned at night
        {
            ChangeState(VillagerState.Combat);
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
        else
        {
            base.Start();
        }
    }

    protected void Update()
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
                if (IsInAttackRange())
                {
                    print($"{villagerData.villagerName} is in attack range of {targetEnemy.name}. Attacking...");
                    attackTimer -= Time.deltaTime;
                    if (attackTimer <= 0)
                    {
                        AttackTarget();
                        attackTimer = attackSpeed;
                        print($"{villagerData.villagerName} attacked {targetEnemy.name}.");
                    }
                }
                else if (villagerState == VillagerState.Combat)
                {
                    print($"{villagerData.villagerName} is moving towards {targetEnemy.name}.");
                    VillagerMoveTo(targetEnemy.transform.position);
                }
            }
            else if (villagerState == VillagerState.Combat)
            {
                print($"{villagerData.villagerName} has no target and is now patrolling.");
                VillagerPatrol();
            }
        }
    }

    public virtual void ChangeToCombatMode() // change the combat villager to combat mode
    {
        ChangeState(VillagerState.Combat);
    }

    public virtual void ChangeToPatrolMode() // change the combat villager back to patrol mode
    {
        ChangeState(VillagerState.Patrol);
    }
    protected void SetNewTarget() //method to set new target
    {
        print($"{villagerData.villagerName} is searching for new targets within range {targetDetectRange}.");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, targetDetectRange);

        if (colliders.Length == 0)
        {
            print($"{villagerData.villagerName} found no enemies within range.");
            targetEnemy = null;
            return;
        }

        float minDistance = float.MaxValue;
        GameObject closestEnemy = null;

        for (int i = 0; i < colliders.Length; i++)
        {
            Enemy enemy = colliders[i].gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
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

    protected void AttackTarget() //method to attack the target by using the target's interface
    {
        if (targetEnemy != null)
        {
            SetVillagerDirection(targetEnemy.transform.position.x);
            targetEnemy.GetComponent<IDamageable>().TakeDamage(damage);
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
        return distance <= attackRange;
    }
}
