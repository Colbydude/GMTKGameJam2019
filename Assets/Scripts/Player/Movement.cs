using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Config.
    public float walkSpeed = 10f;
    public float jumpSpeed = 10f;
    public float climbSpeed = 10f;
    public GameObject _ladder;
    public ParticleSystem _deathParticleSystem;

    private float animSpeedAtStart;
    private float gravityScaleAtStart;
    private bool isClimbing = false;
    private bool isGrounded = true;
    private GameObject ladderInstance = null;

    // Component references.
    private Animator _animator;
    private BoxCollider2D _bodyCollider;
    private Rigidbody2D _rigidBody2D;
    private SpriteRenderer _sprite;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _bodyCollider = GetComponent<BoxCollider2D>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();

        animSpeedAtStart = _animator.speed;
        gravityScaleAtStart = _rigidBody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Climb();
        Action();
        FlipSprite();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //todo: better isGrounded detection
        Debug.Log("Player Collided With: " + collision.gameObject.name);
        switch (collision.gameObject.tag)
        {
            case "Ladder":
                isGrounded = true;
                break;
            case "Tilemap_Floor":
                isGrounded = true;
                break;
            case "Tilemap_Damage":
                KillPlayer();
                break;
            case "Enemy":
                //get location of bottom of player
                float playerBottom = _rigidBody2D.transform.position.y - _sprite.bounds.size.y / 2;
                SpriteRenderer enemySprite = collision.gameObject.GetComponent<SpriteRenderer>();
                float enemyTop = collision.transform.position.y + enemySprite.bounds.size.y / 2;

                if (Mathf.Abs(playerBottom - enemyTop) < .25f)
                {
                    Jump();
                    Destroy(collision.gameObject);
                }
                else
                {
                    KillPlayer();
                }
                break;
        }
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

    private void MovePlayer()
    {
        //get axis, should work for keyboard or controller
        float xDir = Input.GetAxis("Horizontal");


        if (Input.GetButton("Jump") && isGrounded)
        {
            Jump();
        }

        //translate character based on speed
        xDir *= walkSpeed;
        _rigidBody2D.velocity = new Vector2(xDir, _rigidBody2D.velocity.y);
    }

    private void Jump()
    {
        float yDir = 0;
        isGrounded = false;
        yDir = jumpSpeed;
        _rigidBody2D.velocity += new Vector2(0, yDir);
    }

    private void KillPlayer()
    {
        ParticleSystem particleSystem = Instantiate(_deathParticleSystem, new Vector3(_rigidBody2D.position.x, _rigidBody2D.position.y, 0), Quaternion.identity);
        particleSystem.Play();
        Destroy(gameObject);
    }

    private void Climb()
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

    private void Action()
    {
        if (Input.GetButtonDown("Action")) {
            if (ladderInstance == null) {
                ladderInstance = Instantiate(_ladder, new Vector3(_rigidBody2D.position.x + transform.localScale.x, _rigidBody2D.position.y, 0), Quaternion.identity);
            } else {
                Destroy(ladderInstance);
                ladderInstance = null;
            }
        }
    }

    private void FlipSprite()
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
