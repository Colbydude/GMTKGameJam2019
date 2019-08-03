using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float gravity;
    public float walkSpeed;
    public float jumpSpeed;

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
        float yDir = gravity * Time.deltaTime;

        //translate character based on speed
        xDir *= (walkSpeed * Time.deltaTime);
        GetComponent<Rigidbody2D>().velocity = new Vector3(xDir, yDir, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision);
    }
}
