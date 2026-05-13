using UnityEngine;
using System;

public class ItemSpotsManager : MonoBehaviour
{
    [Header(" Elemanlar ")]
    [SerializeField] private Transform itemSpot; 
    [SerializeField] private GameObject clickEffectPrefab; 
    [SerializeField] private GameObject friendlyClickEffectPrefab;

    [Header(" Effect Scale Settings ")]
    [SerializeField] private float minEffectScale = 1f;  
    [SerializeField] private float maxEffectScale = 1.2f;
    [SerializeField] private float animationDuration;

    private void Awake()
    {
        InputManager.itemClicked += HandleItemClicked;
    }

    private void OnDestroy()
    {
        InputManager.itemClicked -= HandleItemClicked;
    }

    private void HandleItemClicked(Item item)
    {
        bool isDead = item.TakeDamage();

        if (isDead)
        {
            item.DisablePhysics();

            GameObject effectToSpawn = (item is FriendlyItem) ? friendlyClickEffectPrefab : clickEffectPrefab;

            if (effectToSpawn != null)
            {
                GameObject spawnedEffect = Instantiate(effectToSpawn, item.transform.position, Quaternion.identity);
                
                float randomScaleMultiplier = UnityEngine.Random.Range(minEffectScale, maxEffectScale);
                spawnedEffect.transform.localScale *= randomScaleMultiplier;
            }

            if (item is FriendlyItem)
            {
                // Dost hapsa ceza ver
                Debug.Log("<color=red>  Ceza: Goal Card +=1</color>");
                if (GoalManager.Instance != null)
                {
                    GoalManager.Instance.ApplyPenalty();
                }
            }
            else
            {
                if (GoalManager.Instance != null)
                {
                    GoalManager.Instance.UpdateGoalProgress(item);
                }
            }

            item.ReturnToPool();
        }
        else
        {
            // Virüs ölmediyse yapılacaklar (Örn: 2 HP'li virüslerin ilk hasarı)
        }
    }
}