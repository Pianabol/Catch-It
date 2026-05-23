using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))] 
public class UIButtonAnimator : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 originalScale;

    [Header(" Animation Settings ")]
    [Tooltip("Şişme (Swell) animasyonunun süresi (HC için 0.15f idealdir)")]
    [SerializeField] private float animationDuration = 0.15f;

    [Tooltip("Orijinal boyutun ne kadar üzerine çıksın? (örn: 1.15f = %15 büyür)")]
    [SerializeField] private float swellScaleMultiplier = 1.15f; // HC Juice oranı

    [Tooltip("Orijinal boyuta geri dönme süresi")]
    [SerializeField] private float returnDuration = 0.12f;

    [Header(" Actions after Animation (GELİŞMİŞ) ")]
    [Tooltip("Animasyon bittikten (şişip-indikten) hemen sonra yapılacak işlem (Örn: Sahneleri yükle)")]
    [SerializeField] private UnityEvent onAnimationComplete;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        originalScale = rectTransform.localScale;
    }

    public void AnimateClick()
    {
        LeanTween.cancel(rectTransform.gameObject);

        LeanTween.scale(rectTransform.gameObject, originalScale * swellScaleMultiplier, animationDuration)
            .setEase(LeanTweenType.easeOutBack) // Hafif sekerek (Juice) büyüsün
            .setOnComplete(() =>
            {
                // Obje imha olmadıysa devam et (Safety Check)
                if (rectTransform != null)
                {
                    LeanTween.scale(rectTransform.gameObject, originalScale, returnDuration)
                        .setEase(LeanTweenType.easeInSine) // Yumuşak bir geçiş
                        .setOnComplete(() => {
                            onAnimationComplete?.Invoke();
                        });
                }
            });
    }
}