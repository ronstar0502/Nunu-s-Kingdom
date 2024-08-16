using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private float _horizontalInput;
    private Animator _animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal"); //getting input for horizontal movement
    }

    private void FixedUpdate()
    {
        InputMovement();
    }
    private void InputMovement()
    {
        //setting the direction of our movement based on the input
        if (_horizontalInput > 0) 
        {
            rb.velocity = Vector2.right * moveSpeed * Time.fixedDeltaTime;
            sr.flipX = false;
            _animator.SetBool("isWalking",true);
        }
        else if (_horizontalInput < 0)
        {
            rb.velocity = Vector2.left * moveSpeed * Time.fixedDeltaTime;
            sr.flipX = true;
            _animator.SetBool("isWalking", true);
        }
        else
        {
            rb.velocity = Vector2.zero;
            _animator.SetBool("isWalking", false);
        }
    }
}
