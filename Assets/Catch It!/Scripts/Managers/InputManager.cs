using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static Action<Item> itemClicked;

    [Header(" Settings ")]
    private Item currentItem;
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }   
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
