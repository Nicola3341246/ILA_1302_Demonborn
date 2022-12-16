using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PlayerMove : MonoBehaviour
{
    float walkdirection;
    bool jump;
    bool role;
    bool block;
    bool attack;

    [SerializeField] private float gravity;
    [SerializeField] private float walkspeed;
    [SerializeField] private float jumpheight;
    [SerializeField] private float attackRange;

    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Animator playerAnimator;
    private Rigidbody2D body;
    private BoxCollider2D box;
    private Animator anim;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        
        GetMovement();
        FlipPlayer();
        MovePlayer();
        JumpPlayer();
        PlayerAttack();
        if (IsOnWall() && !IsGrounded())
        {
            body.gravityScale = gravity;
        }
    }

    private void GetMovement()
    {
        walkdirection = Input.GetAxis("Horizontal");
        jump = Input.GetKey("space");
        attack = Input.GetKeyDown(KeyCode.Mouse0);
    }

    private void FlipPlayer()
    {
        if (walkdirection > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (walkdirection < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void MovePlayer()
    {
        playerAnimator.SetFloat("Speed", Math.Abs(walkdirection));
        body.velocity = new Vector2(walkdirection * walkspeed, body.velocity.y);
    }

    private bool IsGrounded()
    {
        RaycastHit2D isOnGround = Physics2D.BoxCast(box.bounds.center, box.bounds.size, 0, Vector2.down, 0.01f, groundLayer);
        return isOnGround.collider != null;
    }

    private void JumpPlayer()
    {
        if (jump && IsGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpheight);
        }
    }

    private void PlayerAttack()
    {
        if (attack)
        {
            //Hier animation
            playerAnimator.Play("attack-player");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach(Collider2D enemy in hitEnemies)
            {
                System.Random rd = new System.Random();
                enemy.GetComponent<EnemyHealth>().TakeDamage(rd.Next(minDamage, maxDamage));
            }
        }
    }

    private bool IsOnWall()
    {
        RaycastHit2D isOnGround = Physics2D.BoxCast(box.bounds.center, box.bounds.size, 0, Vector2.down, 0.01f, wallLayer);
        return isOnGround.collider != null;
    }
}
