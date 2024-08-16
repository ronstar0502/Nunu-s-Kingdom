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
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector2.right * enemyData.speed*Time.deltaTime*direction;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("attack");
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }

}
