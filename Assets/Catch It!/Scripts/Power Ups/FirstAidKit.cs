using UnityEngine;
using NaughtyAttributes;

public class FirstAidKit : PowerUp
{
    // Artık 'Activate' metodunun içini doldurmak ZORUNDA
    public override void Activate()
    {
        // O %40'lık algoritmayı buraya gömüyoruz
        
        // Dikkat: LevelManager ve spawnArea referanslarını artık PowerupManager'dan veya Singleton'dan çekmeliyiz.
        // spawnArea'yı bu scriptin Inspector'ından atamak en temizi.
        BoxCollider spawnArea = PowerupManager.Instance.GetSpawnArea(); // Birazdan Manager'a bu metodu ekleyeceğiz
        
        if (LevelManager.Instance == null || spawnArea == null) return;

        Item[] activeViruses = LevelManager.Instance.Items;

        if (activeViruses == null || activeViruses.Length == 0) return;

        float startX = spawnArea.bounds.max.x; 
        float endX = spawnArea.bounds.min.x;   
        float totalWidth = startX - endX;
        float dangerThresholdX = startX - (totalWidth * 0.40f); 

        int destroyedCount = 0;

        foreach (Item virus in activeViruses)
        {
            if (virus == null || !virus.gameObject.activeInHierarchy) continue; 

            if (virus.transform.position.x <= dangerThresholdX)
            {
                if (GoalManager.Instance != null)
                {
                    GoalManager.Instance.UpdateGoalProgress(virus);
                }
                virus.gameObject.SetActive(false); 
                destroyedCount++;
            }
        }
        Debug.Log($"<color=green>FirstAidKit: Tehlike bölgesindeki {destroyedCount} virüs yok edildi!</color>");
    }
}