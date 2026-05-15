using UnityEngine;

public class UIManager : MonoBehaviour, IGameStateListener
{
    [Header(" Panels ")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private GameObject gameOverPanel;

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
                break;
            case EGameState.LEVELCOMPLETE:
                levelCompletePanel.SetActive(true);
                break;
            case EGameState.GAMEOVER:
                gameOverPanel.SetActive(true);
                break;
        }
    }

}
