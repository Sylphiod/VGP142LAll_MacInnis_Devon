using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 10f;
    public float gravity = -9.8f;
    public Transform respawnPoint;
    public GameObject projectilePrefab;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isDead = false;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
        characterController.SimpleMove(moveDirection * speed);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = jumpForce;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Rigidbody rigidbody = projectile.GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward * 20f, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyProjectile"))
        {
            isDead = true;
            transform.position = respawnPoint.position;
            isDead = false;
        }
    }
}
