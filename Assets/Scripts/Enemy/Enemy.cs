using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour , IDamageable
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private float spawnOffsetY;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private int direction = 1;
    private List<GameObject> buildingTargets;
    private GameObject currentBuildingTarget;
    //private IDamageable targetInterface;
    private float attackTimer;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();

        enemyData.health = enemyData.maxHealth;
    }

    private void Start()
    {
        FlipSprite();
        transform.position = new Vector3(transform.position.x, spawnOffsetY, 0f);
        attackTimer = enemyData.attackSpeed;
        SetTarget();
    }

    private void Update()
    {
        if (currentBuildingTarget == null || !currentBuildingTarget.activeInHierarchy)
        {
            SetTarget();
        }

        if (currentBuildingTarget != null)
        {
            if (IsInAttackRange())
            {
                _rb.velocity = Vector2.zero;
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    AttackTarget();
                    attackTimer = enemyData.attackSpeed;
                }
            }
        }

    }



    private void FixedUpdate()
    {
        if (currentBuildingTarget != null && !IsInAttackRange())
        {
            _rb.velocity = new Vector2(direction * enemyData.movementSpeed * Time.deltaTime,_rb.velocity.y);
        }
        else
        {
            _rb.velocity = Vector2.zero;
        }
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        print("attack");
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable TMPtarget))
        {
            StopMovement();
            currentTarget = collision.gameObject;
            targetInterface = TMPtarget;
            Attacking();
        }
    }*/

    private void FlipSprite()
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
    private void SetTarget()
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
        print($"enemy target: {currentBuildingTarget.gameObject.name}");
    }

    private bool IsInAttackRange()
    {
        if(currentBuildingTarget==null) return false;
        float distance = Mathf.Abs(transform.position.x - currentBuildingTarget.transform.position.x); //with vector2.Distance() had some troubles with the target set and attack range
        return distance <= enemyData.attackRange;
    }

    /*private void CheckAttackRange()
    {
        if (IsInAttackRange())
        {
            StopMovement();
        }
        else
        {
            ResumeMovement();
        }
    }*/

    private void AttackTarget()
    {
        if (currentBuildingTarget != null)
        {
            currentBuildingTarget.GetComponent<IDamageable>().TakeDamage(enemyData.damage);
        }
        else
        {
            SetTarget();
        }
    }

    public void TakeDamage(int damage)
    {
        enemyData.health -= damage;
        print($"{enemyData.name} took {damage} damage and now has {enemyData.health} health");
        if(enemyData.health <= 0)
        {
            Destroy(gameObject);
        }
    }


    /*private void StopMovement()
{
   isWalking = 0;
   print("stopped moving");
}
private void ResumeMovement()
{
   isWalking = 1;
   print("resumed moving");
}*/

    /*private void Attacking()
    {
        if (currentTarget != null)
        {
            targetInterface.TakeDamage(enemyData.dmg);
            Invoke("Attacking", 1f);
        }
        else
        {
            ResumeMovement();
        }
    }*/
}
