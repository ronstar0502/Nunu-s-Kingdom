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
    //private float attackTimer;
    //private bool isInAttackAnimation;

    protected override void Awake()
    {
        base.Awake();
        if (HQ.isNightMode) //has some bugs with new recruited ammigas when spawned at night
        {
            SetState(AmiggaState.Combat);
            SetNewTarget();
            if (targetEnemy != null)
            {
                print($"{amiggaData.villagerName} is spawned at night and switched to combat mode | {amiggaState} and has target: {targetEnemy.name}");
            }
            else
            {
                print($"{amiggaData.villagerName} is spawned at night and switched to combat mode | {amiggaState} but has no target.");
            }
        }
        
    }
    protected override void Start()
    {
        //attackTimer = attackSpeed;
        if (!HQ.isNightMode)
        {
            base.Start();
        }
    }

    protected override void Update()
    {
        base.Update();
        if (amiggaState == AmiggaState.Combat || amiggaState == AmiggaState.InProffesionBuilding)
        {
            print($"{amiggaData.villagerName} is in combat mode.");
            if (targetEnemy == null || !targetEnemy.activeInHierarchy) //if target is not in hierarchy and null sets new target
            {
                print($"{amiggaData.villagerName} has no target or target is inactive. Setting new target.");
                SetNewTarget();
            }

            if (targetEnemy != null)
            {
                print($"{amiggaData.villagerName} found a target: {targetEnemy.name}.");
                SetAmmigarAttackTargetDirection();
                if (IsInAttackRange())
                {
                    animator.SetBool("isAttacking",true);
                    print($"{amiggaData.villagerName} is in attack range of {targetEnemy.name}. Attacking...");

                    /*if (!isInAttackAnimation)
                    {
                        //attackTimer -= Time.deltaTime;
                        //if (attackTimer <= 0)
                        //{
                        //}
                        StartCoroutine(AttackTarget());
                    }*/
                }
                else
                {
                    if (amiggaState == AmiggaState.Combat)
                    {
                        print($"{amiggaData.villagerName} is moving towards {targetEnemy.name}.");
                        animator.SetBool("isAttacking",false);
                        AmiggaMoveToTarget(targetEnemy.transform.position);
                    }
                }
            }
            else if (amiggaState == AmiggaState.Combat)
            {
                print($"{amiggaData.villagerName} has no target and is now patrolling.");
                animator.SetBool("isAttacking", false);
                AmiggaPatrol();
            }
        }
    }
    public void AttackTarget()
    {
        if (targetEnemy != null)
        {
            targetEnemy.GetComponent<IDamageable>().TakeDamage(damage);
            print($"{amiggaData.villagerName} attacked {targetEnemy.name} with {damage} damage.");
        }
    }
    public virtual void ChangeToCombatMode() // change the combat villager to combat mode
    {
        SetState(AmiggaState.Combat);
    }

    public virtual void ChangeToDayMode() // change the combat villager back to patrol mode
    {
        if (targetEnemy == null && !HQ.isNightMode)
        {
            SetState(AmiggaState.Patrol);
            animator.SetBool("isAttacking", false);
        }
    }
    protected void SetNewTarget() //method to set new target
    {
        print($"{amiggaData.villagerName} is searching for new targets within range {targetDetectRange}.");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, targetDetectRange);

        if (colliders.Length == 0)
        {
            print($"{amiggaData.villagerName} found no enemies within range.");
            targetEnemy = null;
            if (!HQ.isNightMode)
            {
                SetState(AmiggaState.Patrol);
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
            print($"{amiggaData.villagerName} found a new target: {targetEnemy.name}.");
        }
        else
        {
            targetEnemy = null;
            print($"{amiggaData.villagerName} did not find any valid enemies.");
        }
    }

    protected virtual void SetAmmigarAttackTargetDirection() //method to attack the target by using the target's interface
    {
        if (targetEnemy != null)
        {
            SetAmmigaDirection(targetEnemy.transform.position.x);
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
