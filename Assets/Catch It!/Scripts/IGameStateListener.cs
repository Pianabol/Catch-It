using UnityEngine;

public interface IGameStateListener
{
    void GameStateChanged(EGameState newState);
}
