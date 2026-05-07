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
    [SerializeField] private float levelDuration = 90f; // Saniye cinsinden bölüm süresi (1.5 dk = 90s)

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

        // Toplam süreyi, çıkacak toplam virüs sayısına bölüyoruz. 
        // Böylece bölüm boyunca eşit aralıklarla düşecekler.
        float spawnInterval = levelDuration / itemsToSpawnList.Count;

        // Listeyi tek tek dön ve virüsleri fırlat
        foreach (Item prefab in itemsToSpawnList)
        {
            SpawnSingleItem(prefab);
            
            // Bir sonraki virüse kadar hesapladığımız interval kadar bekle
            yield return new WaitForSeconds(spawnInterval); 
        }
    }

    private void SpawnSingleItem(Item prefab)
    {
        // BoxCollider'ın sınırları (Bounds) içinde rastgele bir X, Y, Z noktası bul
        Bounds bounds = spawnArea.bounds;
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);

        // Şimdilik Instantiate ile yaratıyoruz. Pooling'e geçtiğimizde burayı Pool.Get() olarak değiştireceğiz.
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}