using UnityEngine;
using System;

public class ItemSpotsManager : MonoBehaviour
{
    [Header(" Eleman ")]
    [SerializeField] private Transform itemSpot;

    [Header(" Settings")]
    [SerializeField] private Vector3 itemLocalPosition;
    [SerializeField] private Vector3 itemLocalScale;


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
        // Handle item click logic here
        // ıvır zıvır.

        item.transform.SetParent(itemSpot);
        item.transform.localPosition = itemLocalPosition;
        item.transform.localScale = itemLocalScale;
        
        item.DisableShadows();
        item.DisablePhysics();
    }

     
}
