using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static Action<Item> itemClicked;
    public static Action<PowerUp> powerupClicked;
    
    [Header(" Settings ")]
    private Item currentItem;
    [SerializeField] private LayerMask powerUpLayer;
    void Update()
    {
        if(GameManager.Instance.IsGame())
        {
            HandleControl();
        }
    }
    
    private void HandleControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (HandlePowerUpClick()) 
            {
                return; 
            }
            HandleClick();
        }   
    }

    private bool HandlePowerUpClick()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 30, powerUpLayer);
        
        if (hit.collider == null) return false;
        if (!hit.collider.TryGetComponent(out PowerUp powerUp)) return false;
        
        
        Debug.Log($"<color=yellow>[Raycast Hit]</color> PowerUp Objesi: <b>{hit.collider.name}</b> | Enum Tipi: <color=cyan>{powerUp.Type}</color>");
        
        powerupClicked?.Invoke(powerUp);
        return true; 
    }

    private void HandleClick()
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 30);
        if(hit.collider == null)
        {
            return;
        }
        if(!hit.collider.TryGetComponent(out Item item))
        {
            return;
        }

        Debug.Log("Hit: " + hit.collider.name);
        itemClicked?.Invoke(item);
    }


}
