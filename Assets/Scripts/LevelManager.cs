using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string nextScene;
    [SerializeField] private GameObject pausePanel;

    void OnEnable()
    {
        GameEvents.PlayerWonEvent += OnPlayerWin;
        GameEvents.GameStateChangedEvent += OnStateChange;
    }

    void OnDisable()
    {
        GameEvents.PlayerWonEvent -= OnPlayerWin;
        GameEvents.GameStateChangedEvent -= OnStateChange;
    }

    void Start()
    {
        GameStateManager.SetState(GameState.Playing);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameStateManager.GetCurrentState() == GameState.Playing)
            {
                GameStateManager.SetState(GameState.Paused);
            }
            else if (GameStateManager.GetCurrentState() == GameState.Paused)
            {
                GameStateManager.SetState(GameState.Playing);
            }
        }
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameStateManager.SetState(GameState.Playing);
    }

    void OnPlayerWin()
    {
        if (nextScene != "")
        {
            SceneManager.LoadScene(nextScene);
        }
        else
        {
            SceneManager.LoadScene("WinScene");
        }
    }

    void OnGUI()
    {
        
    }

    void OnStateChange(GameState newState)
    {
        if (newState == GameState.Playing)
        {
            pausePanel.SetActive(false);
        }
        else if (newState == GameState.Paused)
        {   
            pausePanel.SetActive(true);
        }
    }
}
