using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkSpeed;
    public float jumpSpeed;

    private bool isGrounded = true;

    private Animator _animator;
    private Rigidbody2D _rigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
        flipSprite();
    }

    void movePlayer()
    {
        //get axis, should work for keyboard or controller
        float xDir = Input.GetAxis("Horizontal");
        float yDir = 0;

        if (isGrounded)
        {
            if (Input.GetAxis("Jump") != 0 && isGrounded)
            {
                isGrounded = false;
                yDir = jumpSpeed * Input.GetAxis("Jump");
                _rigidBody2D.velocity += new Vector2(0, yDir);
                Debug.Log(Input.GetAxis("Jump"));
            }
        }

        //translate character based on speed
        xDir *= walkSpeed;
        _rigidBody2D.velocity = new Vector2(xDir, _rigidBody2D.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //todo: better isGrounded detection
        Debug.Log(collision);
        isGrounded = true;
    }

    private void flipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_rigidBody2D.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(_rigidBody2D.velocity.x), transform.localScale.y);
            _animator.Play("PlayerMove");
        } else {
            _animator.Play("PlayerIdle");
        }
    }
}
