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
    private bool isInAttackAnimation;

    protected void Awake()
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
        attackTimer = attackSpeed;
        base.Start();
    }

    protected void Update()
    {
        base.Update();
        if (amiggaState == AmiggaState.Combat || amiggaState == AmiggaState.InProffesionBuilding)
        {
            //print($"{villagerData.villagerName} is in combat mode.");
            if (targetEnemy == null || !targetEnemy.activeInHierarchy) //if target is not in hierarchy and null sets new target
            {
                //print($"{villagerData.villagerName} has no target or target is inactive. Setting new target.");
                SetNewTarget();
            }

            if (targetEnemy != null)
            {
                //print($"{villagerData.villagerName} found a target: {targetEnemy.name}.");
                if (IsInAttackRange())
                {
                    animator.SetBool("isAttacking",true);
                    //print($"{villagerData.villagerName} is in attack range of {targetEnemy.name}. Attacking...");
                    if (!isInAttackAnimation)
                    {
                        attackTimer -= Time.deltaTime;
                        if (attackTimer <= 0)
                        {
                            StartCoroutine(AttackTargetTimer());
                        }
                    }
                }
                else if (amiggaState == AmiggaState.Combat)
                {
                    animator.SetBool("isAttacking",false);
                    //print($"{villagerData.villagerName} is moving towards {targetEnemy.name}.");
                    AmiggaMoveToTarget(targetEnemy.transform.position);
                }
            }
            else if (amiggaState == AmiggaState.Combat)
            {
                //print($"{villagerData.villagerName} has no target and is now patrolling.");
                AmiggaPatrol();
            }
        }
    }
    private IEnumerator AttackTargetTimer()
    {
        AttackTarget();
        if (targetEnemy != null)
        {
            isInAttackAnimation = true;
            yield return new WaitUntil(()=>animator.GetCurrentAnimatorStateInfo(0).IsName($"{amiggaData.villagerName}Attack"));
            print($"{amiggaData.villagerName} finished animation");
            attackTimer = attackSpeed;
            isInAttackAnimation = false;
        }
    }

    public void DealDamageToTarget()
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

    public virtual void ChangeToPatrolMode() // change the combat villager back to patrol mode
    {
        SetState(AmiggaState.Patrol);
    }
    protected void SetNewTarget() //method to set new target
    {
        print($"{amiggaData.villagerName} is searching for new targets within range {targetDetectRange}.");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, targetDetectRange);

        if (colliders.Length == 0)
        {
            print($"{amiggaData.villagerName} found no enemies within range.");
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

    protected virtual void AttackTarget() //method to attack the target by using the target's interface
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
        return distance <= attackRange;
    }
}
