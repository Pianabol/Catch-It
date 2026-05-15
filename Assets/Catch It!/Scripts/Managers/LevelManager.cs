using UnityEngine;
using System;

public class LevelManager : MonoBehaviour, IGameStateListener
{
    [Header( " Levels ")]
    [SerializeField] private Level[] levels;
    private const string levelKey= "LevelReached";
    private int levelIndex;

    [Header(" Settings ")]
    private Level currentLevel; 

    [Header(" Actions ")]
    public static Action<Level> levelSpawned;

    private void Awake()
    {
        LoadData();
    }

    void Start()
    {
        
    }
    
    private void SpawnLevel()
    {
        transform.Clear();

        if(levels.Length <= 0)
        {
            Debug.LogError("LevelManager: No levels assigned in the inspector!");
            return;
        }

        int validatedIndex = Mathf.Clamp(levelIndex, 0, levels.Length - 1);
        currentLevel = Instantiate(levels[validatedIndex], transform);
        
        levelSpawned?.Invoke(currentLevel);
        currentLevel.StartLevel();

    }

    private void LoadData()
    {
        levelIndex = PlayerPrefs.GetInt(levelKey);
    }
    private void SaveData()
    {
        PlayerPrefs.SetInt(levelKey, levelIndex);
    }

    public void GameStateChanged(EGameState newState)
    {
        if (newState == EGameState.GAME)
        {
            SpawnLevel();
        }       
        else if (newState == EGameState.LEVELCOMPLETE)
        {
            levelIndex++;
            SaveData();
        }
    }
}
