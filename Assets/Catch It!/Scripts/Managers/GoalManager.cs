using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System; 

[System.Serializable]
public class GoalData
{
    public Item itemPrefab;          
    public int targetAmount;         
    
    [HideInInspector] public int currentAmount;  
    [HideInInspector] public bool isCompleted;  
    public int RemainingAmount => Mathf.Max(0, targetAmount - currentAmount);
}

public class GoalManager : MonoBehaviour
{
    public static GoalManager Instance; 
    public List<GoalData> activeGoals = new List<GoalData>();
    public static event Action<GoalData> OnGoalUpdated;       
    public static event Action<GoalData> OnGoalCompleted;     
    public static event Action OnLevelCompleted;              

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

    private void OnEnable()
    {
        LevelManager.levelSpawned += OnLevelSpawned;
    }

    private void OnDisable()
    {
        LevelManager.levelSpawned -= OnLevelSpawned;
    }

    private void OnLevelSpawned(Level spawnedLevel)
    {
        // Level doğduğu anda içindeki hedefleri çek!
        SetLevelGoals(spawnedLevel.GetGoals());
        Debug.Log($"<color=green> GoalManager: Yeni bölüm sinyali alındı, hedefler yüklendi.</color>");
    }

    public void SetLevelGoals(List<GoalData> goalsFromLevel)
    {
        activeGoals.Clear();
        
        foreach (var goal in goalsFromLevel)
        {
            activeGoals.Add(new GoalData 
            { 
                itemPrefab = goal.itemPrefab, 
                targetAmount = goal.targetAmount 
            });
        }
        
        Debug.Log("Bölüm hedefleri LevelManager'dan başarıyla yüklendi!");
    }

    public void UpdateGoalProgress(Item item)
    {
        string cleanName = item.name.Replace("(Clone)", "").Trim();
        GoalData goal = activeGoals.FirstOrDefault(g => g.itemPrefab.name == cleanName);

        if (goal != null && !goal.isCompleted)
        {
            goal.currentAmount++;
            
            OnGoalUpdated?.Invoke(goal);
            Debug.Log($"<color=cyan> Goal Card Güncel Veri: {goal.itemPrefab.name} -> Kalan: {goal.RemainingAmount}</color>");

            if (goal.currentAmount >= goal.targetAmount)
            {
                goal.isCompleted = true;
                
                OnGoalCompleted?.Invoke(goal);
                Debug.Log($"<color=green> HEDEF TAMAMLANDI: {goal.itemPrefab.name}</color>");

                CheckLevelWinCondition();
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
                OnGoalUpdated?.Invoke(goal);
            }
        }
        Debug.Log("<color=yellow>  Ceza! Aktif hedeflerin tamamlanma şartı +1 arttı!</color>");
    }

    private void CheckLevelWinCondition()
    {
        bool allDone = activeGoals.All(g => g.isCompleted);
        if (allDone)
        {
            Debug.Log("<color=magenta>  BÜTÜN HEDEFLER BİTTİ! BÖLÜM GEÇİLDİ!</color>");
            OnLevelCompleted?.Invoke(); 
        }
    }
}