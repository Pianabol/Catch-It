using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    // UIManager'ın dinleyeceği sinyaller
    public static event Action<int, int> OnScoreUpdated; // (Toplam Puan, Eklenen/Çıkarılan Miktar)
    public static event Action<int> OnComboUpdated;      // (Mevcut Kombo Sayısı)
    public static event Action OnComboBroken;            // (Kombo Sıfırlandı)

    [Header(" Score Data ")]
    private int currentScore;
    private int currentCombo = 1;
    public int CurrentScore => currentScore;
    
    [Header(" Combo Settings ")]
    [Tooltip("Komboyu devam ettirmek için geçmesi gereken maksimum süre")]
    [SerializeField] private float comboWindow = 0.5f;
    private float comboTimer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // Kombo sayacı aktifse süreden düş
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            
            // Süre dolduysa komboyu sıfırla
            if (comboTimer <= 0)
            {
                ResetCombo();
            }
        }
    }

    public void ProcessItemScore(int baseScore)
    {
        int scoreChange = 0;

        if (baseScore > 0) // VİRÜS VURULDU (Pozitif)
        {
            // Puanı kombo ile çarp
            scoreChange = baseScore * currentCombo;
            currentScore += scoreChange;
            
            // Komboyu artır ve süreyi başa sar
            currentCombo++;
            comboTimer = comboWindow; 
            
            // UIManager ekrana "x2, x3" yazdırsın diye sinyal yolla
            if (currentCombo > 1) 
            {
                OnComboUpdated?.Invoke(currentCombo);
            }
        }
        else // DOST HAP VURULDU (Negatif - Ceza)
        {
            scoreChange = baseScore; // Zaten Inspector'dan -50 falan girdin
            currentScore += scoreChange;
            
            if (currentScore < 0) currentScore = 0; // Puan eksiye düşmesin
            
            ResetCombo(); // Ceza yediğin an kombo acımadan sıfırlanır!
        }

        // UIManager'a "Skor değişti, animasyonu patlat" sinyali yolla
        OnScoreUpdated?.Invoke(currentScore, scoreChange);
    }

    private void ResetCombo()
    {
        if (currentCombo > 1)
        {
            OnComboBroken?.Invoke();
        }
        currentCombo = 1;
        comboTimer = 0f;
    }
     public void ResetScore()
    {
        currentScore = 0;
        ResetCombo();
        
        OnScoreUpdated?.Invoke(currentScore, 0);
    }
}