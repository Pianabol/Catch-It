using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIIdleBreather : MonoBehaviour
{
    private RectTransform rectTransform;

    [Header(" Breathe Settings ")]
    [Tooltip("Buton durduğu yerde yüzde kaç büyüsün? ")]
    [SerializeField] private float breatheScaleMultiplier = 1.05f; 

    [Tooltip("Bir tam nefes alma (büyüme) süresi ne kadar sürsün?")]
    [SerializeField] private float breatheDuration = 0.8f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        
        Vector3 originalScale = rectTransform.localScale;
        Vector3 targetScale = originalScale * breatheScaleMultiplier;

        LeanTween.scale(rectTransform.gameObject, targetScale, breatheDuration)
            .setEase(LeanTweenType.easeInOutSine) // easeInOutSine gerçek bir nefes alışverişi gibi yumuşaktır
            .setLoopPingPong(); 
    }
}