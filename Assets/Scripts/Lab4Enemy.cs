using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab4Enemy : MonoBehaviour
{
    public LayerMask whatIsGround;
    public Transform player;
    public float moveSpeed = 3f;
    public float maxSpeed = 5f;
    public float attackRange = 2f;
    public float kickCoolDown = 1f;
    public float punchCoolDown = 2f;
    private float timeSinceLastKick;
    private float timeSinceLastPunch;
    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private bool isHit;
    private bool isAttacking;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
     
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.2f, whatIsGround);

      
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

  
        if (distanceToPlayer <= attackRange && !isHit && !isAttacking)
        {
            float attackChoice = Random.Range(0f, 1f);
            if (attackChoice <= 0.5f && Time.time - timeSinceLastKick > kickCoolDown)
            {
                anim.SetTrigger("Kick");
                isAttacking = true;
                timeSinceLastKick = Time.time;
            }
            else if (Time.time - timeSinceLastPunch > punchCoolDown)
            {
                anim.SetTrigger("Punch");
                isAttacking = true;
                timeSinceLastPunch = Time.time;
            }
        }

       
        if (!isHit && !isAttacking)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
            anim.SetBool("Running", true);

            
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
        }
        else
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("Running", false);
        }

       
        if (!isAttacking && rb.velocity == Vector2.zero)
        {
            anim.SetTrigger("Idle");
        }
    }

    public void OnHit()
    {
        anim.SetTrigger("Hit");
        isHit = true;
        Invoke("ResetHit", 3f);
    }

    public void ResetHit()
    {
        isHit = false;
    }

    public void ResetAttack()
    {
        isAttacking = false;
    }
}

