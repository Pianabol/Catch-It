using UnityEngine;
using System.Collections.Generic;

public class Level : MonoBehaviour
{
    [Header(" Spawners ")]
    [SerializeField] private ItemPlacer itemPlacer;

    [Header(" Level Goals ")]
    [SerializeField] private int duration;
    public int Duration => duration;
    [SerializeField] private List<GoalData> levelGoals; // Bölümün hedefleri artık burada
    public List<GoalData> GetGoals() => levelGoals;

    public void StartLevel()
    {
        if (itemPlacer != null) itemPlacer.StartSpawning();
        // if (friendSpawner != null) friendSpawner.StartSpawning();
        
        Debug.Log("<color=orange>Level: Spawner'lara ateş emri verildi!</color>");
    }

    public Item[] GetItems()
    {
        return itemPlacer.GetItems();
    }
}