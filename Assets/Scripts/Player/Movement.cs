using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkSpeed = 10f;
    public float jumpSpeed = 10f;
    public float climbSpeed = 10f;

    private float animSpeedAtStart;
    private float gravityScaleAtStart;
    private bool isClimbing = false;
    private bool isGrounded = true;

    private Animator _animator;
    private BoxCollider2D _bodyCollider;
    private Rigidbody2D _rigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _bodyCollider = GetComponent<BoxCollider2D>();
        _rigidBody2D = GetComponent<Rigidbody2D>();

        animSpeedAtStart = _animator.speed;
        gravityScaleAtStart = _rigidBody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
        climb();
        flipSprite();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //todo: better isGrounded detection
        Debug.Log("Player Collided With: " + collision.gameObject.name);
        if (collision.gameObject.tag == "Tilemap_Floor")
            isGrounded = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Ladder.ClimbCollider") {
            isClimbing = true;
            _rigidBody2D.velocity = new Vector2(0, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Ladder.ClimbCollider") {
            isClimbing = false;
            _animator.speed = animSpeedAtStart;
            _rigidBody2D.gravityScale = gravityScaleAtStart;
        }
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
            }
        }

        //translate character based on speed
        xDir *= walkSpeed;
        _rigidBody2D.velocity = new Vector2(xDir, _rigidBody2D.velocity.y);
    }

    private void climb()
    {
        if (isClimbing) {
            bool playerHasVerticalSpeed = Mathf.Abs(_rigidBody2D.velocity.y) > Mathf.Epsilon;

            if (playerHasVerticalSpeed) {
                _animator.speed = animSpeedAtStart;
            } else {
                _animator.speed = 0f;
            }

            float yDir = Input.GetAxis("Vertical") * climbSpeed;
            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, yDir);
            _rigidBody2D.gravityScale = 0;

            _animator.Play("PlayerClimb");
        }
    }

    private void flipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_rigidBody2D.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(_rigidBody2D.velocity.x), transform.localScale.y);

            if (!isClimbing) {
                _animator.Play("PlayerMove");
            }
        } else {
            if (!isClimbing) {
                _animator.Play("PlayerIdle");
            }
        }
    }
}
