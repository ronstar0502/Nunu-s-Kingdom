using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private GameObject seedLoot;
    [SerializeField] private Transform lootDropSpot;
    [SerializeField] private SoundEffectManger soundEffectManger;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private float spawnOffsetY;
    [SerializeField] private int lootDropChance;
    private Rigidbody2D _rb;
    private SpriteRenderer _sr;
    private Animator _animator;
    private List<GameObject> _buildingTargets;
    private GameObject _currentBuildingTarget;
    private int _direction = 1;
    private float _attackTimer;
    private float _attackRange;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        soundEffectManger = FindAnyObjectByType<SoundEffectManger>();

        enemyData.health = enemyData.maxHealth;
    }

    private void Start()
    {
        _attackRange = Random.Range(enemyData.minAttackRange, enemyData.maxAttackRange+0.1f);
        SetMovementDirection();
        transform.position = new Vector3(transform.position.x, spawnOffsetY, 0f);
        _attackTimer = enemyData.attackSpeed;
        SetBuildingTarget();
    }

    private void Update()
    {
        if (_currentBuildingTarget == null || !_currentBuildingTarget.activeInHierarchy) //if target is not in hierarchy and null sets new target
        {
            SetBuildingTarget();
        }

        if (_currentBuildingTarget != null)
        {
            SetMovementDirection(_currentBuildingTarget.transform.position.x);
            if (IsInAttackRange())
            {
                _animator.SetBool("isAttacking", true);
                //attackTimer -= Time.deltaTime;
                //if (attackTimer <= 0)
                //{
                //    AttackTarget();
                //    attackTimer = enemyData.attackSpeed;
                //}
            }
            else
            {
                _animator.SetBool("isAttacking", false);
            }
        }

    }

    private void FixedUpdate()
    {
        if (_currentBuildingTarget != null && !IsInAttackRange())
        {
            _rb.velocity = new Vector2(_direction * enemyData.movementSpeed * Time.deltaTime, _rb.velocity.y);
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
            _direction = -1;
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
            _direction = -1;
            _sr.flipX = true;
        }
        else if (targetPoint > transform.position.x)
        {
            _direction = 1;
            _sr.flipX = false;
        }
    }

    private void SetBuildingTarget()
    {
        _buildingTargets = GameObject.FindGameObjectsWithTag("Building").ToList();
        float minDistance = float.MaxValue;
        for (int i = 0; i < _buildingTargets.Count; i++)
        {
            float distance = Vector2.Distance(transform.position, _buildingTargets[i].transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                _currentBuildingTarget = _buildingTargets[i];
            }
        }
        if (_currentBuildingTarget != null)
        {
            print($"enemy target: {_currentBuildingTarget.gameObject.name}");
        }
    }

    private bool IsInAttackRange()
    {
        if (_currentBuildingTarget == null) return false;
        float distance = Mathf.Abs(transform.position.x - _currentBuildingTarget.transform.position.x); //with vector2.Distance() had some troubles with the target set and attack range
        return distance <= _attackRange;
    }

    private void AttackTarget()
    {
        if (_currentBuildingTarget != null)
        {
            if(attackSound != null && soundEffectManger != null)
            {
                soundEffectManger.PlaySFX(attackSound);
            }
            _currentBuildingTarget.GetComponent<IDamageable>().TakeDamage(enemyData.damage);
            print($"{enemyData.enemyName} attacked {_currentBuildingTarget.name} with {enemyData.damage} damage.");
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
