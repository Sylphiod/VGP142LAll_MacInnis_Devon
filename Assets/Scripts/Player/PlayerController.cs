using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Animator anim;

    public Transform projectileSpawn;
    public Rigidbody projectilePrefab;
    public float projectileForce = 10.0f;

    public float moveSpeed = 0;
    public float gravity = 9.81f;
    public float jumpSpeed = 10.0f;

    bool godModeActive = false;
    public float godModeTimer = 3.0f;

    public int health
    {
        get => _health;
        set
        {
            if (godModeActive)
                return;

            _health = value;

            //if (health <= 0)
            //call game over
        }
    }

    int _health;

    CharacterController controller;
    WeaponPickup wp;

    Vector3 curMoveInput;
    Vector2 move;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            controller = GetComponent<CharacterController>();
            controller.minMoveDistance = 0.0f;

            anim = GetComponentInChildren<Animator>();
            wp = GetComponent<WeaponPickup>();

            if (!anim)
            {
                Debug.Log("No animation component in child for the gameobject " + gameObject.name);
            }

            if (moveSpeed <= 0.0f)
            {
                moveSpeed = 6.0f;
                throw new ArgumentNullException("Movespeed Argument is null so the value has been defaulted to 6.0");
            }
        }

        catch (ArgumentNullException e)
        {
            Debug.Log(e.Message);
        }

        catch (UnassignedReferenceException e)
        {
            Debug.Log(e.Message);
        }

        finally
        {
            Debug.Log("This Code Always Runs");
        }

    }

    // Update is called once per frame
    void Update()
    {
        curMoveInput.y -= gravity * Time.deltaTime;
        controller.Move(curMoveInput * Time.deltaTime);

        anim.SetFloat("Forward", move.y);
        anim.SetFloat("Right", move.x);
    }

    public void MovePlayer(InputAction.CallbackContext context)
    {
        //Debug.Log("Move vector is: " + context.action.ReadValue<Vector2>());
        move = context.action.ReadValue<Vector2>();
        move.Normalize();

        Vector3 moveVel = new Vector3(move.x, 0, move.y);
        curMoveInput = moveVel * moveSpeed;

        if (controller.isGrounded)
        {
            curMoveInput = transform.TransformDirection(curMoveInput);
        }
    }



    public void Fire(InputAction.CallbackContext context)
    {
        if (context.action.WasPressedThisFrame())
        {
            if (projectilePrefab && wp.weaponFirePoint && GetComponent<WeaponPickup>().weapon != null)
            {
                Rigidbody temp = Instantiate(projectilePrefab, wp.weaponFirePoint.transform.position, wp.weaponFirePoint.transform.rotation);
                temp.AddForce(wp.weaponFirePoint.transform.forward * projectileForce, ForceMode.Impulse);

                Destroy(temp.gameObject, 2.0f);
            }
        }
    }

    IEnumerator StopGodMode()
    {
        yield return new WaitForSeconds(godModeTimer);

        godModeActive = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("GodMode"))
        {
            if (!godModeActive)
            {
                godModeActive = true;
                StartCoroutine(StopGodMode());
            }

            Destroy(hit.gameObject);
        }

    }
}
