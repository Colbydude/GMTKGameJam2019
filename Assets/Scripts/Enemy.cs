using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 1f;
    public ParticleSystem deathParticleSystem;

    BoxCollider2D _bodyCollider2D;
    Rigidbody2D _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _bodyCollider2D = GetComponent<BoxCollider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        //check to see if he's stuck against a wall, and flip direction if so
        //== should compare approximately
        if (Vector2.zero == _rigidBody.velocity)
            moveSpeed *= -1;

        _rigidBody.velocity = new Vector2(moveSpeed, _rigidBody.velocity.y);

        if (_rigidBody.velocity.x > 0) {
            transform.localScale = new Vector2(-1f, 1f);
        } else {
            transform.localScale = new Vector2(1f, 1f);
        }
    }

    private void OnDestroy()
    {
        ParticleSystem particleSystem = Instantiate(deathParticleSystem, new Vector3(_rigidBody.position.x, _rigidBody.position.y, 0), Quaternion.identity);
        particleSystem.Play();
    }
}
