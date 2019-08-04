using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Config.
    public float walkSpeed = 10f;
    public float jumpSpeed = 10f;
    public float climbSpeed = 10f;
    public GameObject ladderObject;
    public GameObject ladderInstance = null;
    public ParticleSystem deathParticleSystem;

    private float animSpeedAtStart;
    private float gravityScaleAtStart;
    private bool isAttacking = false;
    private bool isClimbing = false;
    private bool isGrounded = true;
    public GameObject ladderTriggerInRange;

    // Component references.
    private Animator _animator;
    private BoxCollider2D _bodyCollider;
    private PolygonCollider2D _hurtBox;
    private Rigidbody2D _rigidBody2D;
    private SpriteRenderer _sprite;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _bodyCollider = GetComponent<BoxCollider2D>();
        _hurtBox = this.GetComponentInChildren<PolygonCollider2D>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();

        animSpeedAtStart = _animator.speed;
        gravityScaleAtStart = _rigidBody2D.gravityScale;

        _hurtBox.enabled = false;
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
            case "ThrowableLadder":
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
                    if (isAttacking) {
                        Destroy(collision.gameObject);
                    } else {
                        KillPlayer();
                    }
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Climbable") {
            isClimbing = true;
            _rigidBody2D.velocity = new Vector2(0, 0);
        } else if (collision.gameObject.tag == "LadderTrigger") {
            ladderTriggerInRange = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Climbable") {
            isClimbing = false;
            _animator.speed = animSpeedAtStart;
            _rigidBody2D.gravityScale = gravityScaleAtStart;
        } else if (collision.gameObject.tag == "LadderTrigger") {
            ladderTriggerInRange = null;
        }
    }

    private void MovePlayer()
    {
        //get axis, should work for keyboard or controller
        float xDir = Input.GetAxis("Horizontal");


        if (Input.GetButtonDown("Jump") && isGrounded) {
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
        ParticleSystem particleSystem = Instantiate(deathParticleSystem, new Vector3(_rigidBody2D.position.x, _rigidBody2D.position.y, 0), Quaternion.identity);
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
            if (ladderInstance == null && ladderTriggerInRange == null) {
                // Swing ladder as a weapon.
                _animator.Play("PlayerAttack");
                _rigidBody2D.velocity = new Vector2(0, 0);
                isAttacking = true;
                _hurtBox.enabled = true;

            } else if (ladderTriggerInRange != null) {
                if (ladderInstance != null)
                    RecallLadder();

                // Spawn ladder in context space.
                LadderTrigger trigger = ladderTriggerInRange.GetComponent<LadderTrigger>();
                ladderInstance = trigger.spawnLadder();
            } else if (ladderInstance != null) {
                RecallLadder();
            }
        }

        //Throw the ladder
        if (Input.GetButtonDown("Fire1"))
        {
            if (ladderInstance == null)
            {
                ladderInstance = Instantiate(ladderObject, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(new Vector3(0, 0, 90)));
                ladderInstance.gameObject.GetComponent<Rigidbody2D>().velocity += _rigidBody2D.transform.localScale * new Vector2(10, 1);
            }
            else
            {
                RecallLadder();
            }
        }
    }

    private void RecallLadder()
    {
        // "Recall" the ladder.
        Debug.Log("Destroying...");
        Destroy(ladderInstance);
        ladderInstance = null;
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(_rigidBody2D.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed) {
            transform.localScale = new Vector2(Mathf.Sign(_rigidBody2D.velocity.x), transform.localScale.y);

            if (!isClimbing && !isAttacking) {
                if (ladderInstance != null) {
                    _animator.Play("PlayerMove");
                } else {
                    _animator.Play("PlayerMoveLadder");
                }
            }
        } else {
            if (!isClimbing && !isAttacking) {
                _animator.Play("PlayerIdle");
            }
        }
    }

    public void AlertObservers(string message)
    {
        if (message.Equals("AttackAnimationEnded")) {
            isAttacking = false;
            _hurtBox.enabled = false;
        }
    }
}
