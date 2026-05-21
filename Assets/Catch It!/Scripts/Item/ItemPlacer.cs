using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;

public class ItemPlacer : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private List<ItemLevelData> itemDatas;
    [SerializeField] private List<Item> friendlyPrefabs; 

    [Header(" Settings ")]
    [SerializeField] private BoxCollider spawnArea;
    // levelDuration'ı Level scriptinden alacaz.

    [Header(" Fruit Ninja Tempo Settings ")]
    [SerializeField] private float minWaveDelay = 1.5f;
    [SerializeField] private float maxWaveDelay = 3f;
    [SerializeField] private int minItemsPerWave = 3;
    [SerializeField] private int maxItemsPerWave = 5;
    [SerializeField] private float microDelayBetweenItems = 0.1f;

    [Header(" Data ")]
    private Item[] items; 
    private List<Item> itemsToSpawnList = new List<Item>();
    
    private float currentLevelDuration; 

    public Item[] GetItems()
    {
        return GetComponentsInChildren<Item>().Where(x => x.gameObject.activeInHierarchy).ToArray();
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
        currentLevelDuration = spawnedLevel.Duration;
    }

    [Button]
    public void StartSpawning()
    {
        PrepareSpawnList();
        StartCoroutine(SpawnRoutine());
    }

    private void PrepareSpawnList()
    {
        itemsToSpawnList.Clear();

        foreach (var data in itemDatas)
        {
            for (int i = 0; i < data.amount; i++)
            {
                itemsToSpawnList.Add(data.itemPrefab);
            }
        }

        for (int i = 0; i < itemsToSpawnList.Count; i++)
        {
            Item temp = itemsToSpawnList[i];
            int randomIndex = Random.Range(i, itemsToSpawnList.Count);
            itemsToSpawnList[i] = itemsToSpawnList[randomIndex];
            itemsToSpawnList[randomIndex] = temp;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        if (itemsToSpawnList.Count == 0) yield break;

        int currentIndex = 0;
        float elapsedTime = 0f;

        while (elapsedTime < currentLevelDuration) 
        {
            int waveSize = Random.Range(minItemsPerWave, maxItemsPerWave + 1);
            int trapIndex = Random.Range(0, waveSize);

            for (int i = 0; i < waveSize; i++)
            {
                if (GoalManager.Instance != null)
                {
                    if (i == trapIndex && friendlyPrefabs != null && friendlyPrefabs.Count > 0)
                    {
                        Item randomFriendly = friendlyPrefabs[Random.Range(0, friendlyPrefabs.Count)];
                        SpawnSingleItem(randomFriendly);
                    }
                    else
                    {
                        bool isPanicTime = (currentLevelDuration - elapsedTime) <= 15f;
                        Item urgentItem = null;

                        if (isPanicTime)
                        {
                            urgentItem = GoalManager.Instance.GetUrgentMissingItem();
                        }

                        if (urgentItem != null)
                        {
                            SpawnSingleItem(urgentItem);
                        }
                        else
                        {
                            if (currentIndex >= itemsToSpawnList.Count)
                            {
                                PrepareSpawnList();
                                currentIndex = 0;
                            }

                            Item prefab = itemsToSpawnList[currentIndex];
                            
                            if (GoalManager.Instance.ShouldSpawn(prefab))
                            {
                                SpawnSingleItem(prefab);
                            }
                            
                            currentIndex++; 
                        }
                    }
                }
                
                yield return new WaitForSeconds(microDelayBetweenItems); 
            }

            float nextWaveWait = Random.Range(minWaveDelay, maxWaveDelay);
            yield return new WaitForSeconds(nextWaveWait);

            elapsedTime += nextWaveWait + (waveSize * microDelayBetweenItems);
        }
    }

    private void SpawnSingleItem(Item prefab)
    {
        Bounds bounds = spawnArea.bounds;
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        float fixedY = 1.5f; 

        Vector3 spawnPosition = new Vector3(randomX, fixedY, randomZ);

        Item spawnedItem = PoolManager.Instance.GetItem(prefab, spawnPosition);
        spawnedItem.Initialize(); 
    }
}