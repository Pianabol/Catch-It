using TMPro;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.Threading;

public class TimerManager : MonoBehaviour, IGameStateListener
{
    public static TimerManager Instance;
    [Header(" Timer Settings ")]
    [SerializeField] private TextMeshProUGUI timerText;
    private int remainingTime;

    private Coroutine freezeCoroutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

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
   
    #region Freeze Mechanic (Zamanı Dondurma)
    public void FreezeTimer(float duration)
    {
        // Eğer zaten donmuşsa ve oyuncu tekrar basarsa, eski sayacı sıfırla ki süre uzasın
        if (freezeCoroutine != null) StopCoroutine(freezeCoroutine);
        
        freezeCoroutine = StartCoroutine(FreezeRoutine(duration));
    }

    private IEnumerator FreezeRoutine(float duration)
    {
        StopTimer(); 

        Color originalColor = Color.white; // Varsayılan rengin beyaz olduğunu varsayıyoruz
        timerText.color = new Color(0.5f, 0.8f, 1f); 

        // TODO: İleride Container'a buz çatlama efekti (Image/Sprite) falan da buraya eklenecek.
        Debug.Log($"<color=cyan>TimerManager: Zaman {duration} saniyeliğine BUZ TUTTU!</color>");

        yield return new WaitForSeconds(duration);

        // Buzu çöz, her şeyi eski haline getir ve zamanı tekrar başlat
        timerText.color = originalColor;
        StartTimer();
        Debug.Log("<color=orange>TimerManager: Buz çözüldü, zaman tekrar akıyor...</color>");
    }

    #endregion

}
