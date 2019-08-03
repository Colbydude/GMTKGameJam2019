using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkSpeed;
    public float jumpSpeed;

    private bool isGrounded = true;

    // Start is called before the first frame update
    void Start()
    {
        
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
                Debug.Log(Input.GetAxis("Jump"));
            }

        }

        //translate character based on speed
        xDir *= walkSpeed;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(xDir, yDir));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //todo: better isGrounded detection
        Debug.Log(collision);
        isGrounded = true;
    }
}
