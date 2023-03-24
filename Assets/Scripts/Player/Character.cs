using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class Character : MonoBehaviour, IDatatPersistence
{
    CharacterController controller;
    Animator anim;

    [Header("Player Settings")]
    [Space(10)]
    [Tooltip("Speed value between 1 and 20")]
    [Range(1.0f, 20.0f)]
    public float speed = 20;
    public float gravity = 9.81f;
    public float jumpSpeed = 10.0f;

    [HideInInspector] public ObjectSounds sfxManager;
    public AudioClip shootSound;
    public AudioClip punchSound;
    public AudioClip kickSound;
    public AudioMixerGroup soundFXGroup;



    Vector3 moveDir;

    [Header("Weapon Settings")]
    [Space(10)]
    public float projectileForce = 10.0f;
    public Rigidbody projectilePrefab;
    public Transform projectileSpawnPoint;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        controller.minMoveDistance = 0.0f;

        if (speed <= 0.0f)
        {
            speed = 20.0f;

        }

        if (jumpSpeed <= 0)
        {
            jumpSpeed = 10.0f;
        }
    }


    public void LoadData (GameData data)
    {
        this.transform.position = data.playerPosition;
    }

    public void SaveData (ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        try
        {

            if (controller.isGrounded)

            {
                moveDir = new Vector3(horizontal, 0.0f, vertical);
                moveDir *= speed;

                moveDir = transform.TransformDirection(moveDir);

                if (Input.GetButtonDown("Jump"))
                {
                    moveDir.y = jumpSpeed;
                    anim.SetTrigger("Jump");
                }

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    anim.SetTrigger("Punch");
                    sfxManager.Play(punchSound, soundFXGroup);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    anim.SetTrigger("Kick");
                    sfxManager.Play(kickSound, soundFXGroup);
                }
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    anim.SetTrigger("Fire");

                }
            }

            moveDir.y -= gravity * Time.deltaTime;
            controller.Move(moveDir * Time.deltaTime);

            anim.SetFloat("horizontal", horizontal);
            anim.SetFloat("vertical", vertical);

            if (Input.GetButtonDown("Fire1"))
                Fire1();
            throw new UnassignedReferenceException("Shooting Disabled " + name + " Shooting Enabled " + Input.GetButtonDown("Fire1"));
        }
        finally
        {
            Debug.Log("Shooting has been Enabled");
        }
    }

    void Fire1()
    {
        anim.SetTrigger("Fire");
        if (projectilePrefab && projectileSpawnPoint)
        {
            Rigidbody temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

            temp.AddForce(projectileSpawnPoint.forward * projectileForce, ForceMode.Impulse);

            Destroy(temp.gameObject, 2.0f);
        }
    }



    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Coin")
        {
            Destroy(hit.gameObject);
        }

        if (hit.gameObject.tag == "Enemy")
        {
            SceneManager.LoadScene("GameOver");
        }

        if (hit.gameObject.tag == "Water")
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "J_Powerup")
        {
            jumpSpeed += 5.0f;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "M_Powerup")
        {
            speed += 5.0f;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "W_Powerup")
        {
            projectileForce += 5.0f;
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "A_Powerup")
        {
            projectileForce += 2.0f;
            speed += 2.0f;
            jumpSpeed += 5.0f;
            Destroy(other.gameObject);
        }
    }
}