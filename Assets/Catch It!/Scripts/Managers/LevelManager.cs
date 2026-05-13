using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [Header(" Spawners ")]
    [SerializeField] private ItemPlacer itemPlacer;
    [SerializeField] private ItemSpawner friendSpawner;

    [Header(" Level Goals ")]
    [SerializeField] private List<GoalData> levelGoals; // Bölümün hedefleri artık burada

    private void Start()
    {
        if (GoalManager.Instance != null)
        {
            GoalManager.Instance.SetLevelGoals(levelGoals);
        }
    
    }
    public List<GoalData> GetGoals() => levelGoals;
}