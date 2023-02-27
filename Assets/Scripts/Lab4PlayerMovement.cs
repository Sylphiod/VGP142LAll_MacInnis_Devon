using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lab4PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public KeyCode jumpKey = KeyCode.Space;
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public Transform orientation;
    public Animator animator;
    public GameObject playerPunch;
    public GameObject playerKick;
    private Animator anim;

    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        SpeedControl();

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

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Z))
        {
            playerPunch.SetActive(true);
            animator.SetTrigger("Punch");
            Invoke("DisablePunch", 0.2f);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            playerKick.SetActive(true);
            animator.SetTrigger("Kick");
            Invoke("DisableKick", 0.2f);
        }

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void DisablePunch()
    {
        playerPunch.SetActive(false);
    }

    private void DisableKick()
    {
        playerKick.SetActive(false);
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        float currentSpeed = moveSpeed;
        if (!grounded)
            currentSpeed *= airMultiplier;

        Vector3 newPosition = transform.position + moveDirection.normalized * currentSpeed * Time.deltaTime;
        rb.MovePosition(newPosition);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}