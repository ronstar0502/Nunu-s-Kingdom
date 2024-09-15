using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D _rb;
    private GameObject target;
    private Vector2 targetDirection;
    private int arrowDamage;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (target != null)
        {
            _rb.velocity = targetDirection * speed * Time.deltaTime;
        }
    }
    public void InitArrow(GameObject targetObj, Vector2 direction, int damage ,bool isFromTower)
    {
        if (targetObj != null)
        {
            target = targetObj;
            targetDirection = direction;
            if (isFromTower)
            {
                targetDirection.Normalize();
                float arrowRotation = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                _rb.rotation = arrowRotation;

                targetDirection = direction; //preserve the original direction so the speed wont change dramatically
            }
            arrowDamage = damage;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target && target !=null)
        {
            print($"arrow hit target: {collision.gameObject.name} with {arrowDamage} damage");
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(arrowDamage);
            Destroy(gameObject);
        }
    }
}
