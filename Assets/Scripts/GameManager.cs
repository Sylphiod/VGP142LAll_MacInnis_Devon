using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : Singelton<GameManager>
{
    [HideInInspector]
    public Player playerInput;

    MouseLook playerLook;
    MouseLook cameraLook;

    PlayerController controller;
    protected override void Awake()
    {
        base.Awake();
        playerInput = new Player();

    }
    // Start is called before the first frame update
    void Start()
    {
        playerInput.Actions.Escape.performed += ctx => OnButtonPress(ctx);
        AddPlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    // Update is called once per frame
    public void OnButtonPress(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (SceneManager.GetActiveScene().name == "Level")
                SceneManager.LoadScene("MainMenu");
            else
            {
                SceneManager.LoadScene("Level");
            }
        }
    }

    private void LateUpdate()
    {
        if (controller == null && SceneManager.GetActiveScene().name == "Level")
        {
            AddPlayerInput();
        }
    }

    void AddPlayerInput()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        cameraLook = Camera.main.GetComponent<MouseLook>();
        playerLook = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>();


        if (controller == null || cameraLook == null || playerLook == null)
        {

            return;
        }


        playerInput.Actions.Move.performed += ctx => controller.MovePlayer(ctx);
        playerInput.Actions.Move.canceled += ctx => controller.MovePlayer(ctx);
        playerInput.Actions.Fire.performed += ctx => controller.Fire(ctx);

        playerInput.Actions.Look.performed += ctx => cameraLook.Look(ctx);
        playerInput.Actions.Look.performed += ctx => playerLook.Look(ctx);
    }

    public void EndGame()
    {
        SceneManager.LoadScene("Victory");
    }
}