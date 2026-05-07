using UnityEngine;
using System;

public class ItemSpotsManager : MonoBehaviour
{
    [Header(" Elemanlar ")]
    [SerializeField] private Transform itemSpot; 
    [SerializeField] private GameObject clickEffectPrefab; 

    [Header(" Settings")]
    [SerializeField] private float animationDuration = 0.4f;

    private void Awake()
    {
        InputManager.itemClicked += HandleItemClicked;
    }

    private void OnDestroy()
    {
        InputManager.itemClicked -= HandleItemClicked;
    }

   
    private void HandleItemClicked(Item item)
    {
        item.DisablePhysics();

        if (clickEffectPrefab != null)
        {
            Instantiate(clickEffectPrefab, item.transform.position, Quaternion.identity);
        }

        item.ReturnToPool();
    }
}