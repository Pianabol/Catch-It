using UnityEngine;
using System.Collections.Generic;

public class FirstAidKit : PowerUp 
{
    [Header(" First Aid Settings ")]
    [SerializeField] private BoxCollider killArea; 

    public override void Activate() 
    {
        if (killArea == null) 
        {
            Debug.LogWarning("<color=yellow>FirstAidKit: Kill Area (Küp) atanmamış!</color>");
            return;
        }

        Item[] activeViruses = Object.FindObjectsByType<Item>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        if (activeViruses == null || activeViruses.Length == 0) return;

        int destroyedCount = 0;
        Bounds killBounds = killArea.bounds;

        foreach (Item virus in activeViruses) 
        {
            if (virus == null || !virus.gameObject.activeInHierarchy) continue; 

            if (virus.itemType == EItemType.Friendly) continue;

            if (killBounds.Contains(virus.transform.position)) 
            {
                if (GoalManager.Instance != null)
                {
                    GoalManager.Instance.UpdateGoalProgress(virus);
                }

                // 2. ÇİFTE TIKLAMA KORUMASI
                Collider col = virus.GetComponent<Collider>();
                if (col != null) col.enabled = false;

                // 3. LEANTWEEN ŞOVU (Titre ve Küçül)
                LeanTween.cancel(virus.gameObject);

                LeanTween.moveLocalX(virus.gameObject, virus.transform.localPosition.x + 0.3f, 0.3f).setEasePunch();
                
                LeanTween.scale(virus.gameObject, Vector3.zero, 0.3f).setEaseInBack().setOnComplete(() =>
                {
                    virus.gameObject.SetActive(false); 
                    virus.transform.localScale = Vector3.one;
                    if (col != null) col.enabled = true;
                });
                
                destroyedCount++;
            }
        }
        
        Debug.Log($"<color=green>First Aid Kit: Sadece düşman olan {destroyedCount} virüs temizlendi, dostlar güvende!</color>");
    }
}