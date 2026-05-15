using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Threading;

public class TimerManager : MonoBehaviour, IGameStateListener
{
    [Header(" Timer Settings ")]
    [SerializeField] private TextMeshProUGUI timerText;
    private int remainingTime;

    private void Awake()
    {
        LevelManager.levelSpawned += OnLevelSpawned;
    }

    private void OnDestroy()
    {
        LevelManager.levelSpawned -= OnLevelSpawned;
    }

    private void OnLevelSpawned(Level level)
    {
        remainingTime = level.Duration;
        UpdateTimerText();

        StartTimer();
    }

    private void StartTimer()
    {
        InvokeRepeating("UpdateTimer", 0, 1);
    }

    private void UpdateTimer()
    {
        remainingTime--;
        UpdateTimerText();

        if (remainingTime <= 0)
        {
            TimerEnded();
            Debug.Log("<color=red>TimerManager: Süre doldu, oyun bitti!</color>");
        }
    }

    private void UpdateTimerText()
    {
        timerText.text = SecondsToTimeString(remainingTime);
    }
    private string SecondsToTimeString(int seconds)
    {
        return TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
    }

    private void StopTimer()
    {
        CancelInvoke("UpdateTimer");
    }

    private void TimerEnded()
    {
        StopTimer();
        GameManager.Instance.SetGameState(EGameState.GAMEOVER);
    }

    public void GameStateChanged(EGameState newState)
    {
        if (newState == EGameState.LEVELCOMPLETE || newState == EGameState.GAMEOVER)
        {
            StopTimer();
        }
         
    }

}
