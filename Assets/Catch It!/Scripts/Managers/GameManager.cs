using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private EGameState gameState;

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
        SetGameState(EGameState.MENU);
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

    public void StartGame()
    {
        SetGameState(EGameState.GAME);
    }


    
}
