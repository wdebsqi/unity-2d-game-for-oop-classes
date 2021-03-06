﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region References
    public Rigidbody2D rigidbodyEnemy;
    public BoxCollider2D triggerEnemy;
    public GameObject enemy;
    public EnemyHealthManager enemyHM;

    [Header("Animator")]
    public Animator animator;

    [Header("Particles")]
    public GameObject deathParticles;

    [Header("Layers")]
    public LayerMask GroundLayer;

    [Header("Physics")]
    public Transform EnemyLegPosition;

    Transform leftWall, rightWall;
    #endregion

    #region Boolean variables
    public bool movesRight;
    bool isGrounded;
    bool isFalling = true;

    #endregion

    #region Movement settings
    int leftOrRight;
    [Header("Movement settings")]
    public int movementSpeed = 8;
    #endregion

    #region Physics
    public Vector2 BoxSize;
    #endregion


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
    }

    void Update()
    {
        if (isGrounded)
        {
            animator.SetBool("isRunning", true);
        }

        Flip();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        // Physics - Movement - Checking if box detected colider that is ground
        isGrounded = Physics2D.BoxCast(EnemyLegPosition.position, BoxSize, 0f, Vector2.down, 0, GroundLayer);



        // Physics - Movement - falling when enemy is not grounded
        #region Falling mechanics
        animator.SetBool("isFalling", !isGrounded);

        EnemyFalling();

        isFalling = !isGrounded;
        #endregion

        // if statement - Movement - hitting walls
        #region Hitting walls mechanics
        if (movesRight)
        {
            // check if not hitting right wall
            if (transform.position.x >= rightWall.position.x)
            {
                // if true, go left
                //Debug.Log("Hitting RIGHT, going LEFT!");
                MoveLeft();
            }
            // if not, continue going right
            else
            {
                MoveRight();
            }
        }
        else
        {
            // check if not hitting left wall
            if (transform.position.x <= leftWall.position.x)
            {
                // if true, go right
                //Debug.Log("Hitting LEFT, going RIGHT!");
                MoveRight();
            }
            // if not, continue going left
            else
            {
                MoveLeft();
            }
        }
        #endregion
    }

    // Function - Movement - gets enemy movement direction
    #region EnemyMovementDirection()
    void EnemyMovementDirection()
    {
            // randomly get 0 or 1
            // 0 means that enemy will spawn moving left
            // 1 means that enemy will spawn moving right
            leftOrRight = Random.Range(0, 2);
            if (leftOrRight == 0)
            {
                movesRight = false;
                //Debug.Log("Spawning LEFT moving enemy");
            }
            else
            {
                movesRight = true;
                //Debug.Log("Spawning RIGHT moving enemy");
            }
    }
    #endregion
    // Function - Movement - makes enemy move right 
    #region MoveRight()
    void MoveRight()
    {
        if (isGrounded)
        {
            movesRight = true;
            rigidbodyEnemy.velocity = new Vector2(movementSpeed, rigidbodyEnemy.velocity.y);
        }
        else
        {
            rigidbodyEnemy.velocity = new Vector2(0, rigidbodyEnemy.velocity.y);
        }

    }
    #endregion

    // Function - Movement - makes enemy move left
    #region MoveLeft()
    void MoveLeft()
    {
        if (isGrounded)
        {
            movesRight = false;
            rigidbodyEnemy.velocity = new Vector2(-movementSpeed, rigidbodyEnemy.velocity.y);
        }

        else
        {
            rigidbodyEnemy.velocity = new Vector2(0, rigidbodyEnemy.velocity.y);
        }
    }
    #endregion

    // Function - Movement - enemy falling mechanics
    #region EnemyFalling()
    void EnemyFalling()
    {
        if (isGrounded)
        {
            if (isFalling)
            {
                rigidbodyEnemy.velocity = new Vector2(0, rigidbodyEnemy.velocity.y);
                EnemyMovementDirection();
                isFalling = true;

            }
            isFalling = false;
        }
        
    }
    #endregion

    // Function - Animations - flipping enemy sprite
    #region Flip()
    void Flip()
    {
        // Moving  -- going right 
        if (movesRight)
        {
            transform.localScale = new Vector2(1, 1);
        }
        // Moving  -- going left 
        else
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }
    #endregion

    // Function - OnTriggerEnter - hits player
    #region OnTriggerEnter()
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealthManager playerHM = collision.GetComponent<PlayerHealthManager>();
        if (collision.tag == "Player" && playerHM.isHit == false)
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            player.enemyHittingRb = rigidbodyEnemy;
            playerHM.isHit = true;
            playerHM.TakeDamage(1);
        }
    }
    #endregion

    // Function(Unity Event) - Animations & movement - stopping enemy and playing death animation
    #region BeforeDeath() -- for event
    public void BeforeDeath()
    {
        movementSpeed = 0;
        rigidbodyEnemy.velocity = new Vector2(0, rigidbodyEnemy.velocity.y);
        Instantiate(deathParticles, transform.position, Quaternion.identity);
    }
    #endregion

    // Gizmos - Help - drawing helpful things :)
    #region OnDrawGizmos()
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawCube(EnemyLegPosition.position, BoxSize);
    }
    #endregion
}
