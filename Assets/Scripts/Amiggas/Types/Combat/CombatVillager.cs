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
        base.Start();
        attackTimer = attackSpeed;
    }

    protected void Update()
    {
        base.Update();
        if (villagerState == VillagerState.Combat || villagerState == VillagerState.InProffesionBuilding)
        {
            if (targetEnemy == null || !targetEnemy.activeInHierarchy) //if target is not in hierarchy and null sets new target
            {
                SetNewTarget();
            }

            if (targetEnemy != null)
            {
                if (IsInAttackRange())
                {
                    //rb.velocity = Vector2.zero;
                    attackTimer -= Time.deltaTime;
                    if (attackTimer <= 0)
                    {
                        AttackTarget();
                        attackTimer = attackSpeed;
                    }
                }
                else if(villagerState == VillagerState.Combat)
                {
                    VillagerMoveTo(targetEnemy.transform.position);
                }
            }            
        }
    }
    /*private void FixedUpdate()
    {
        if(villagerState == VillagerState.Combat)
        {
            if (targetEnemy != null && !IsInAttackRange())
            {
                VillagerMoveTo(targetEnemy.transform.position);
                //rb.velocity = new Vector2(direction * villagerData.movementSpeed * Time.deltaTime, rb.velocity.y);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }*/

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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, targetDetectRange);
        float minDistance = float.MaxValue;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.GetComponent<Enemy>() !=null)
            {
                float distance = Vector2.Distance(transform.position, colliders[i].gameObject.transform.position);
                if(distance < minDistance)
                {
                    minDistance = distance;
                    targetEnemy = colliders[i].gameObject;
                }
            }
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
