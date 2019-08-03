using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkSpeed;
    public float jumpSpeed;

    private bool isGrounded = true;
    private Rigidbody2D _rigidBody2D;
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
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
}
