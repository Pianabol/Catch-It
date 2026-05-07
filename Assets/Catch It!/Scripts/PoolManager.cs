using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    // Her yerden rahatça ulaşabilmek için Singleton yapıyoruz
    public static PoolManager Instance { get; private set; }

    // Her farklı virüs türü için ayrı bir havuz (ObjectPool) tutan liste
    private Dictionary<int, ObjectPool<Item>> pools = new Dictionary<int, ObjectPool<Item>>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Spawner bu fonksiyonu çağırıp havuzdan virüs isteyecek
    public Item GetItem(Item prefab, Vector3 position)
    {
        int key = prefab.GetInstanceID();

        // Eğer bu virüs türü için henüz bir havuz yoksa, yeni bir havuz inşa et
        if (!pools.ContainsKey(key))
        {
            pools[key] = new ObjectPool<Item>(
                createFunc: () => {
                    Item newItem = Instantiate(prefab);
                    newItem.PrefabID = key; // Hangi havuza ait olduğunu virüsün kendisine söylüyoruz
                    return newItem;
                },
                actionOnGet: (item) => {
                    item.gameObject.SetActive(true);
                    item.transform.position = position;
                },
                actionOnRelease: (item) => {
                    item.gameObject.SetActive(false); // Havuza dönünce görünmez yap
                },
                actionOnDestroy: (item) => {
                    Destroy(item.gameObject);
                },
                collectionCheck: false, // Performans için kapalı tutuyoruz
                defaultCapacity: 15,
                maxSize: 50
            );
        }

        // Havuzdan objeyi çekip veriyoruz
        return pools[key].Get();
    }

    // Virüsler ölünce bu fonksiyonu çağırıp kendilerini havuza iade edecek
    public void ReturnItem(Item item)
    {
        if (pools.TryGetValue(item.PrefabID, out ObjectPool<Item> pool))
        {
            pool.Release(item);
        }
    }
}