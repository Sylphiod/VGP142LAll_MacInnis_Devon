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
private int health = 10;
private Rigidbody rb;
private Animator anim;
private bool isGrounded;
private bool isHit;
private bool isAttacking;

void Start()
{
    rb = GetComponent<Rigidbody>();
    anim = GetComponent<Animator>();
}

void Update()
{
    // Check if enemy is grounded
    isGrounded = Physics.CheckSphere(transform.position, 0.2f, whatIsGround);

    // Check if the enemy is dead
    if (health <= 0)
    {
        // Stop moving
        rb.velocity = Vector3.zero;
        // Play death animation
        anim.SetTrigger("Death");
        // Destroy the enemy after 3 seconds
        Destroy(gameObject, 3f);
        return;
    }

    // Calculate the distance to the player
    float distanceToPlayer = Vector3.Distance(transform.position, player.position);

    // If within attack range, trigger a random attack
    if (distanceToPlayer <= attackRange && !isHit && !isAttacking)
    {
        float attackChoice = Random.Range(0f, 1f);
        if (attackChoice <= 0.5f && Time.time - timeSinceLastKick > kickCoolDown)
        {
            anim.SetTrigger("Kick");
            timeSinceLastKick = Time.time;
            isAttacking = true;
            StartCoroutine(AttackCooldown());
        }
        else if (Time.time - timeSinceLastPunch > punchCoolDown)
        {
            anim.SetTrigger("Punch");
            timeSinceLastPunch = Time.time;
            isAttacking = true;
            StartCoroutine(AttackCooldown());
        }
    }
    if (!isHit && !isAttacking)
    {
        Vector3 moveDirection = (player.position - transform.position).normalized;
        float yVelocity = rb.velocity.y;
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, yVelocity, moveDirection.z * moveSpeed);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        if (rb.velocity.magnitude > 0.1f)
        {
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
            anim.SetTrigger("Idle");
        }
    }
}

private void OnTriggerEnter(Collider other)
{
    if (other.gameObject.tag == "Projectile")
    {
        health -= 1;
        anim.SetTrigger("Hit");
        isHit = true;
        rb.velocity = Vector3.zero;
        StartCoroutine(HitCooldown());
    }
    else if (other.gameObject.tag == "PlayerPunch")
    {
        health -= 2;
        anim.SetTrigger("Hit");
        isHit = true;
        rb.velocity = Vector3.zero;
        StartCoroutine(HitCooldown());
    }
    else if (other.gameObject.tag == "PlayerKick")
    {
        health -= 3;
        anim.SetTrigger("Hit");
        isHit = true;
        rb.velocity = Vector3.zero;
        StartCoroutine(HitCooldown());
    }
}

IEnumerator AttackCooldown()
{
    yield return new WaitForSeconds(0.5f);
    isAttacking = false;
}

IEnumerator HitCooldown()
{
    yield return new WaitForSeconds(0.5f);
    isHit = false;
}
}