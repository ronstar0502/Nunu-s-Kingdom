using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private GameObject seedLoot;
    [SerializeField] private Transform lootDropSpot;
    [SerializeField] private int lootDropChance;
    [SerializeField] private float spawnOffsetY;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Animator animator;
    private List<GameObject> buildingTargets;
    private GameObject currentBuildingTarget;
    private int direction = 1;
    private float attackTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        enemyData.health = enemyData.maxHealth;
    }

    private void Start()
    {
        SetMovementDirection();
        transform.position = new Vector3(transform.position.x, spawnOffsetY, 0f);
        attackTimer = enemyData.attackSpeed;
        SetBuildingTarget();
    }

    private void Update()
    {
        if (currentBuildingTarget == null || !currentBuildingTarget.activeInHierarchy) //if target is not in hierarchy and null sets new target
        {
            SetBuildingTarget();
        }

        if (currentBuildingTarget != null)
        {
            SetMovementDirection(currentBuildingTarget.transform.position.x);
            if (IsInAttackRange())
            {
                animator.SetBool("isAttacking", true);
                //attackTimer -= Time.deltaTime;
                //if (attackTimer <= 0)
                //{
                //    AttackTarget();
                //    attackTimer = enemyData.attackSpeed;
                //}
            }
            else
            {
                animator.SetBool("isAttacking", false);
            }
        }

    }
    private void FixedUpdate()
    {
        if (currentBuildingTarget != null && !IsInAttackRange())
        {
            _rb.velocity = new Vector2(direction * enemyData.movementSpeed * Time.deltaTime, _rb.velocity.y);
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }
    private void SetMovementDirection()
    {
        if (transform.position.x > 0)
        {
            direction = -1;
            _sr.flipX = true;
        }
        else
        {
            _sr.flipX = false;
        }
    }

    private void SetMovementDirection(float targetPoint)
    {
        if (targetPoint < transform.position.x)
        {
            direction = -1;
            _sr.flipX = true;
        }
        else if (targetPoint > transform.position.x)
        {
            direction = 1;
            _sr.flipX = false;
        }
    }
    private void SetBuildingTarget()
    {
        buildingTargets = GameObject.FindGameObjectsWithTag("Building").ToList();
        float minDistance = float.MaxValue;
        for (int i = 0; i < buildingTargets.Count; i++)
        {
            float distance = Vector2.Distance(transform.position, buildingTargets[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                currentBuildingTarget = buildingTargets[i];
            }
        }
        if (currentBuildingTarget != null)
        {
            print($"enemy target: {currentBuildingTarget.gameObject.name}");
        }
    }

    private bool IsInAttackRange()
    {
        if (currentBuildingTarget == null) return false;
        float distance = Mathf.Abs(transform.position.x - currentBuildingTarget.transform.position.x); //with vector2.Distance() had some troubles with the target set and attack range
        return distance <= enemyData.attackRange;
    }

    private void AttackTarget()
    {
        if (currentBuildingTarget != null)
        {          
            currentBuildingTarget.GetComponent<IDamageable>().TakeDamage(enemyData.damage);
            print($"{enemyData.enemyName} attacked {currentBuildingTarget.name} with {enemyData.damage} damage.");
        }
    }

    public void TakeDamage(int damage)
    {
        enemyData.health -= damage;
        print($"{enemyData.enemyName} took {damage} damage and now has {enemyData.health} health");
        if (enemyData.health <= 0)
        {
            int dropPercent = Random.Range(1, 101);
            if(dropPercent <= lootDropChance)
            {
                Instantiate(seedLoot, lootDropSpot.position,Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}
