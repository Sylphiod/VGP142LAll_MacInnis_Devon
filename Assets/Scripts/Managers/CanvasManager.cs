using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{

    [Header("Buttons")]
    public Button startButton;
    public Button settingsButton;
    public Button backButton;
    public Button quitButton;
    public Button returnToMenuButton;
    public Button returnToGameButton;
    public Button saveGameButton;
    public Button newGameButton;
    public Button loadGameButton;

    [Header("Menus")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject pauseMenu;

    [Header("Text")]
    public Text volSliderText;

    [Header("Slider")]
    public Slider volSlider;

    public void StartGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void ShowMainMenu()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ShowSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    void OnSliderValueChanged(float value)
    {
        if (volSliderText)
            volSliderText.text = value.ToString();
    }

    void Start()
    {
        if (startButton)
            startButton.onClick.AddListener(() => StartGame());

        if (settingsButton)
            settingsButton.onClick.AddListener(() => ShowSettingsMenu());

        if (backButton)
            backButton.onClick.AddListener(() => ShowMainMenu());

        if (newGameButton)
            newGameButton.onClick.AddListener(() => FreshGame());

        if (loadGameButton)
            loadGameButton.onClick.AddListener(() => UpdateGame());

        if (saveGameButton)
            saveGameButton.onClick.AddListener(() => SavingGame());

        if (volSlider)
        {
            volSlider.onValueChanged.AddListener((value) => OnSliderValueChanged(value));
            if (volSliderText)
                volSliderText.text = volSlider.value.ToString();

        }
        if (quitButton)
            quitButton.onClick.AddListener(() => QuitGame());

        if (returnToMenuButton)
            returnToMenuButton.onClick.AddListener(() => ReturnToMenu());

        if (returnToGameButton)
            returnToGameButton.onClick.AddListener(() => ReturnToGame());
    }
    void Update()
    {
        if (pauseMenu)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                pauseMenu.SetActive(!pauseMenu.activeSelf);


                if (pauseMenu.activeSelf)
                {
                    Time.timeScale = 0;
                    GameManager.instance.playerInstance.GetComponent<Character>().enabled = false;
                    GameManager.instance.playerInstance.GetComponent<Projectile>().enabled = false;
                }
                else
                {
                    Time.timeScale = 1;
                    GameManager.instance.playerInstance.GetComponent<Character>().enabled = true;
                    GameManager.instance.playerInstance.GetComponent<Projectile>().enabled = true;

                }
            }
        }
    }
    public void FreshGame()
    {
        DataPersistenceManager.instance.NewGame();
    }

    public void UpdateGame()
    {
        DataPersistenceManager.instance.LoadGame();
    }

    public void SavingGame()
    {
        DataPersistenceManager.instance.SaveGame();
    }

    public void ReturnToGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        GameManager.instance.playerInstance.GetComponent<Character>().enabled = true;
        GameManager.instance.playerInstance.GetComponent<Projectile>().enabled = true;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Start");
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
