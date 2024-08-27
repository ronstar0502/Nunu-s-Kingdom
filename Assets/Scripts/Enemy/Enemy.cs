using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]EnemyData enemyData;
    [SerializeField]float spawnOffsetY;
    Rigidbody2D rb;
    SpriteRenderer sr;
    private int direction = 1;
    GameObject target;
    IDamageable targetInterface;
    private bool isAttacking;
    private int isWalking = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (transform.position.x > 0)
        {
            direction = -1;
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
        transform.position = new Vector3(transform.position.x, spawnOffsetY, 0f);
    }
    private void FixedUpdate()
    {
        rb.velocity = Vector2.right * enemyData.speed*Time.deltaTime*direction*isWalking;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("attack");
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.TryGetComponent<IDamageable>(out IDamageable TMPtarget))
        {
            StopMovement();
            target = collision.gameObject;
            targetInterface = TMPtarget;
            Attacking();
        }
    }
    private void StopMovement()
    {
        isWalking = 0;
    }
    private void ResumeMovement()
    {
        isWalking = 1;
    }

    private void Attacking()
    {
        if (target != null)
        {
            targetInterface.TakeDamage(enemyData.dmg);
            Invoke("Attacking", 1f);
        }
        else
        {
            ResumeMovement();
        }
    }
}
