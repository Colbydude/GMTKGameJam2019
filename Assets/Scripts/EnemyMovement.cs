﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    BoxCollider2D _BodyCollider2D;
    Rigidbody2D _RigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _BodyCollider2D = GetComponent<BoxCollider2D>();
        _RigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        _RigidBody.velocity = new Vector2(moveSpeed, 0f);

        if (_RigidBody.velocity.x > 0) {
            transform.localScale = new Vector2(-1f, 1f);
        } else {
            transform.localScale = new Vector2(1f, 1f);
        }
    }
}