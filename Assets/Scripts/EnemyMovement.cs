﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    bool movesRight;
    int leftOrRight;
    public int movementSpeed = 8;
    public Rigidbody2D rigidbodyEnemy;
    public Animator animator;

    Transform leftWall, rightWall;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // create new variables holding Left and Right Wall
        leftWall = GameObject.Find("Left Wall").GetComponent<Transform>();
        rightWall = GameObject.Find("Right Wall").GetComponent<Transform>();

        // randomly get 0 or 1
        // 0 means that enemy will spawn moving left
        // 1 means that enemy will spawn moving right
        leftOrRight = Random.Range(0, 2);
        if (leftOrRight == 0)
        {
            movesRight = false;
            Debug.Log("Spawning LEFT moving enemy");
        }
        else
        {
            movesRight = true;
            Debug.Log("Spawning RIGHT moving enemy");
        }
    }

    void Update()
    {
        if (movesRight)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movesRight)
        {
            // check if not hitting right wall
            if (transform.position.x >= rightWall.position.x)
            {
                // if true, go left
                Debug.Log("Hitting RIGHT, going LEFT!");
                moveLeft();
            }
            // if not, continue going right
            else
            {
                moveRight();
            }
        }
        else
        {
            // check if not hitting left wall
            if (transform.position.x <= leftWall.position.x)
            {
                // if true, go right
                Debug.Log("Hitting LEFT, going RIGHT!");
                moveRight();
            }
            // if not, continue going left
            else
            {
                moveLeft();
            }
        }
    }


    void moveRight()
    {
        movesRight = true;
        rigidbodyEnemy.velocity = new Vector2(movementSpeed, rigidbodyEnemy.velocity.y);
    }


    void moveLeft()
    {
        movesRight = false;
        rigidbodyEnemy.velocity = new Vector2(-movementSpeed, rigidbodyEnemy.velocity.y);
    }
}