using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    protected override void Awake()
    {
        base.Awake();

    }
    [HideInInspector] public Character playerInstance;



    void Start()
    {

    }


    void Update()
    {
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SceneManager.GetActiveScene().name == "Start")
                    SceneManager.LoadScene("Level");
                else if (SceneManager.GetActiveScene().name == "GameOver")
                    SceneManager.LoadScene("Start");
            }
        }
    }

    public void EndGame()
    {
        SceneManager.LoadScene("Victory");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
