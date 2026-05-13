using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class GoalData
{
    public Item itemPrefab;          
    public int targetAmount;         
    
    [HideInInspector] public int currentAmount;  
    [HideInInspector] public bool isCompleted;  
}

public class GoalManager : MonoBehaviour
{
    public static GoalManager Instance; 

    [Header(" Goal Cards ")]
    public List<GoalData> activeGoals = new List<GoalData>();

    private void Awake()
    {
        // Singleton Kurulumu
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateGoalProgress(Item item)
    {
        string cleanName = item.name.Replace("(Clone)", "").Trim();
        GoalData goal = activeGoals.FirstOrDefault(g => g.itemPrefab.name == cleanName);

        if (goal != null && !goal.isCompleted)
        {
            goal.currentAmount++;
            
            Debug.Log($"<color=cyan> Goal Card Güncel Veri: {goal.itemPrefab.name} -> {goal.currentAmount}/{goal.targetAmount}</color>");

            if (goal.currentAmount >= goal.targetAmount)
            {
                goal.isCompleted = true;
                Debug.Log($"<color=green> HEDEF TAMAMLANDI: {goal.itemPrefab.name}</color>");
            }
        }
    }

    public bool ShouldSpawn(Item prefab)
    {
        GoalData goal = activeGoals.FirstOrDefault(g => g.itemPrefab.name == prefab.name);
        
        if (goal == null) return true; 
        
        return !goal.isCompleted;
    }

    public void ApplyPenalty()
    {
        foreach (var goal in activeGoals)
        {
            if (!goal.isCompleted)
            {
                goal.targetAmount++;  
            }
        }
        Debug.Log("<color=yellow> Ceza! Aktif hedeflerin tamamlanma şartı +1 arttı!</color>");
    }

    
}