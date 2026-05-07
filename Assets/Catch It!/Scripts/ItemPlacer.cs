using System;
using System.Collections.Generic;
using UnityEngine;
public class ItemPlacer : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private List<ItemLevelData> itemDatas;

    [Header(" Settings ")]
    [SerializeField] private BoxCollider spawnArea;
    
}
