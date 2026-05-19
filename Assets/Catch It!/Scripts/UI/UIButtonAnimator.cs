using UnityEngine;

[RequireComponent(typeof(RectTransform))] 
public class UIButtonAnimator : MonoBehaviour
{
    [Header(" Animation Settings ")]
    [SerializeField] private float bobbingAmount = 15f; 
    [SerializeField] private float bobbingSpeed = 0.75f; 

    [SerializeField] private float scaleAmount = 1.1f; 
    [SerializeField] private float scaleSpeed = 0.6f; 

    private RectTransform rectTransform;
    private float originalY;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        originalY = rectTransform.anchoredPosition.y;
        LeanTween.cancel(gameObject);
        rectTransform.localScale = Vector3.one;

        // 1. Bobbing (Yukarı-Aşağı Hareket)
        // anchoredPosition.y kullanarak UI sisteminin kafasını karıştırmadan hareket ettiriyoruz
        LeanTween.moveY(rectTransform, originalY + bobbingAmount, bobbingSpeed)
            .setEase(LeanTweenType.easeInOutQuad)
            .setLoopPingPong(-1); // -1 sonsuz döngü demektir!

        // 2. Pulse (Nefes Alma / Büyüyüp Küçülme)
        LeanTween.scale(gameObject, Vector3.one * scaleAmount, scaleSpeed)
            .setEase(LeanTweenType.easeInOutSine)
            .setLoopPingPong(-1);
    }

    private void OnDisable()
    {
        // Panel veya buton kapandığında LeanTween'i durdur ki arka planda hafıza yemesin (Memory Leak koruması)
        LeanTween.cancel(gameObject);
        
        // Pozisyonu ve boyutu orijinal haline geri getir
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, originalY);
        rectTransform.localScale = Vector3.one;
    }
}