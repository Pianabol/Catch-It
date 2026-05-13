using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ItemPlacer : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private List<ItemLevelData> itemDatas;

    [Header(" Settings ")]
    [SerializeField] private BoxCollider spawnArea;
    [SerializeField] private float levelDuration = 90f;

    // Çıkacak tüm virüsleri tutacağımız geçici liste
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

        // 1. Torbayı Doldur: Inspector'daki listeye bakıp istenen adet kadar prefab'ı listeye ekliyoruz.
        foreach (var data in itemDatas)
        {
            for (int i = 0; i < data.amount; i++)
            {
                itemsToSpawnList.Add(data.itemPrefab);
            }
        }

        // 2. Torbayı Karıştır (Shuffle): Hep aynı tür virüsler arka arkaya düşmesin diye listeyi rastgele karıştırıyoruz.
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

        // Listeyi tek tek dön ve virüsleri fırlat
        foreach (Item prefab in itemsToSpawnList)
        {
            // GoalManager sahnedeyse ve "Bunu hala spawn etmelisin" diyorsa fırlat.
            if (GoalManager.Instance != null && GoalManager.Instance.ShouldSpawn(prefab))
            {
                SpawnSingleItem(prefab);
            }
            else
            {
    
            }
            
            yield return new WaitForSeconds(spawnInterval); 
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