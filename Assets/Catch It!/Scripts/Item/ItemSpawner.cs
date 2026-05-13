using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes; // Buton için gerekli eklenti

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public class FriendlySpawnData
    {
        public Item itemPrefab;
        public int amount;
    }

    [Header(" Elements ")]
    [SerializeField] private List<FriendlySpawnData> friendlyItems;

    [Header(" Settings ")]
    [SerializeField] private BoxCollider spawnArea;
    [SerializeField] private float levelDuration = 90f; 
    [SerializeField] private float fixedYPosition = 2f;

    private List<Item> itemsToSpawnList = new List<Item>();

    [Button]
    private void StartSpawning()
    {
        PrepareSpawnList();
        StartCoroutine(SpawnRoutine());
    }

    private void PrepareSpawnList()
    {
        itemsToSpawnList.Clear();
        foreach (var data in friendlyItems)
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

        float spawnInterval = levelDuration / itemsToSpawnList.Count;

        // Listeyi tek tek dön ve fırlat
        foreach (Item prefab in itemsToSpawnList)
        {
            SpawnSingleItem(prefab);
            
            yield return new WaitForSeconds(spawnInterval); 
        }
    }

    private void SpawnSingleItem(Item prefab)
    {
        Bounds bounds = spawnArea.bounds;
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        Vector3 spawnPosition = new Vector3(randomX, fixedYPosition, randomZ);

        Item spawnedItem = PoolManager.Instance.GetItem(prefab, spawnPosition);
        spawnedItem.Initialize();
    }
}