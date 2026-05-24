using UnityEngine;
using TMPro; 

public class UIManager : MonoBehaviour, IGameStateListener
{
    [Header(" Panels ")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject gameOverPanel;

    [Header(" Score UI Elements ")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RectTransform pointDiv;
    [SerializeField] private TextMeshProUGUI levelPopupText;

    [Header(" End Game Score Texts ")]
    [SerializeField] private TextMeshProUGUI levelCompleteScoreText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;

    private Vector3 originalPointDivPos; 
    private Vector3 originalPointDivScale; 

    private void Start()
    {
        if (pointDiv != null) 
        {
            originalPointDivPos = pointDiv.localPosition;
            originalPointDivScale = pointDiv.localScale; // Sahnede ne ayarladıysan onu kaydeder
            
            if (scoreText != null) scoreText.text = ""; 
        }
    }

    private void OnEnable()
    {
        ScoreManager.OnScoreUpdated += UpdateScoreUI;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreUpdated -= UpdateScoreUI;
    }

    public void GameStateChanged(EGameState newState)
    {
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        switch (newState)
        {
            case EGameState.MENU:
                mainMenuPanel.SetActive(true);
                break;
            case EGameState.GAME:
                gamePanel.SetActive(true);
                ShowLevelPopup();
                break;
            case EGameState.LEVELCOMPLETE:
                levelCompletePanel.SetActive(true);
                if (ScoreManager.Instance != null && levelCompleteScoreText != null)
                {
                    levelCompleteScoreText.text = "Score : " + ScoreManager.Instance.CurrentScore.ToString();
                }
                if (AudioManager.Instance != null) AudioManager.Instance.PlayLevelCompleteSound();
                break;
            case EGameState.GAMEOVER:
                gameOverPanel.SetActive(true);
                if (ScoreManager.Instance != null && gameOverScoreText != null)
                {
                    gameOverScoreText.text = "Score : " + ScoreManager.Instance.CurrentScore.ToString();
                }
                if (AudioManager.Instance != null) AudioManager.Instance.PlayGameOverSound();
                break;
        }
    }

    private void UpdateScoreUI(int totalScore, int scoreChange)
    {
        if (scoreText == null || pointDiv == null) return;

        if (totalScore == 0 && scoreChange == 0)
        {
            scoreText.text = "";
            return;
        }
        else
        {
            scoreText.text = totalScore.ToString();
        }

        LeanTween.cancel(pointDiv.gameObject);
        pointDiv.localPosition = originalPointDivPos;
        pointDiv.localScale = originalPointDivScale; // Vector3.one YERİNE KENDİ BOYUTU!

        if (scoreChange > 0)
        {
            // Orijinal boyutun %30'u kadar büyüsün
            Vector3 targetScale = originalPointDivScale * 1.3f; 
            
            LeanTween.scale(pointDiv.gameObject, targetScale, 0.12f)
                .setEase(LeanTweenType.easeOutBack)
                .setOnComplete(() =>
                {
                    if (pointDiv != null)
                    {
                        LeanTween.scale(pointDiv.gameObject, originalPointDivScale, 0.12f)
                            .setEase(LeanTweenType.easeInSine);
                    }
                });
        }
        else if (scoreChange < 0)
        {
            LeanTween.moveLocalX(pointDiv.gameObject, originalPointDivPos.x + 15f, 0.2f).setEasePunch();
            
            LeanTween.cancel(scoreText.gameObject); 
            scoreText.color = Color.white; 

            LeanTween.value(scoreText.gameObject, Color.white, Color.red, 0.12f)
                .setLoopPingPong(1)
                .setOnUpdate((Color c) => {
                    if (scoreText != null) scoreText.color = c;
                });
        }
    }

    private void ShowLevelPopup()
    {
        if (levelPopupText == null || LevelManager.Instance == null) return;

        levelPopupText.text = "LEVEL " + LevelManager.Instance.CurrentLevelNum.ToString();

        levelPopupText.gameObject.SetActive(true);
        levelPopupText.transform.localScale = Vector3.zero;

        LeanTween.scale(levelPopupText.gameObject, Vector3.one, 0.4f)
            .setEase(LeanTweenType.easeOutBack) // Jöle gibi şişer
            .setOnComplete(() =>
            {
                LeanTween.scale(levelPopupText.gameObject, Vector3.zero, 0.3f)
                    .setDelay(1.2f) 
                    .setEase(LeanTweenType.easeInBack)
                    .setOnComplete(() => 
                    {
                        levelPopupText.gameObject.SetActive(false);
                    });
            });
    }
}