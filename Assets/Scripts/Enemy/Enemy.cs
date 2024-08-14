using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]EnemyData enemyData;
    Rigidbody2D rb;
    SpriteRenderer sr;
    private int direction = 1;

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
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.right * enemyData.speed*Time.deltaTime*direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("attack");
    }

}
