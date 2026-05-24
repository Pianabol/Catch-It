using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private EGameState gameState;

    public static bool startInGameMode = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (startInGameMode)
        {
            startInGameMode = false; 
            StartGame();
        }
        else
        {
            SetGameState(EGameState.MENU);
        }
    }

    public void SetGameState(EGameState newState)
    {
        this.gameState = newState;

        IEnumerable<IGameStateListener> gameStateListeners
            = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IGameStateListener>();

        foreach (IGameStateListener dependency in gameStateListeners)
        {
            dependency.GameStateChanged(newState);
        }
    }

    public bool IsGame()
    {
        return gameState == EGameState.GAME;
    }

    public void StartGame()
    {
        if (ScoreManager.Instance != null) ScoreManager.Instance.ResetScore();
        
        SetGameState(EGameState.GAME);
    }

    public void HomeButtonCallBack()
    {
        if (CanvasFader.Instance != null)
        {
            CanvasFader.Instance.FadeOut(() => {
                startInGameMode = false; 
                SceneManager.LoadScene(0);
            });
        }
        else
        {
            startInGameMode = false;
            SceneManager.LoadScene(0);
        }
    }

    public void NextButtonCallBack()
    {
        int nextLevelIndex = PlayerPrefs.GetInt("CurrentLevel", 0) + 1;
        PlayerPrefs.SetInt("CurrentLevel", nextLevelIndex);
        PlayerPrefs.Save();

        if (CanvasFader.Instance != null)
        {
            CanvasFader.Instance.FadeOut(() => {
                startInGameMode = true;  
                SceneManager.LoadScene(0);
            });
        }
        else
        {
            startInGameMode = true;
            SceneManager.LoadScene(0);
        }
    }

    public void RetryButtonCallBack()
    {
        if (CanvasFader.Instance != null)
        {
            CanvasFader.Instance.FadeOut(() => {
                startInGameMode = true;  
                SceneManager.LoadScene(0);
            });
        }
        else
        {
            startInGameMode = true;
            SceneManager.LoadScene(0);
        }
    }
}