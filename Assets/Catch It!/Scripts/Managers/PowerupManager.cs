using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        InputManager.powerupClicked += OnPowerUpClicked;
    }

    private void OnDestroy()
    {
        InputManager.powerupClicked -= OnPowerUpClicked;
    }

    private void OnPowerUpClicked(PowerUp clickedPowerUp)
    {
        clickedPowerUp.UsePowerUp();
    }
}