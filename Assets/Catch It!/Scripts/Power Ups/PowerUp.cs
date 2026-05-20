using TMPro;
using UnityEngine;

public enum EPowerUpType
{
    FirstAidKit = 0,
    FreezeGun = 1,
    Shield = 2
}

public abstract class PowerUp : MonoBehaviour
{
    [Header(" Core Settings ")]
    [SerializeField] private EPowerUpType type;
    public EPowerUpType Type => type;

    [Header(" Usage Settings ")]
    [SerializeField] protected int remainingUses = 3; 
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    [Header(" Elemanlar ")]
    [SerializeField] private TextMeshPro amountText;

    private void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        originalScale = transform.localScale;

        UpdateVisuals(remainingUses);
    }

    public void UpdateVisuals(int amount)
    {
       if (amountText != null)
        {
            amountText.gameObject.SetActive(amount > 0);
            amountText.text = amount.ToString();
            
            LeanTween.cancel(amountText.gameObject);
            amountText.transform.localScale = Vector3.one;
            LeanTween.scale(amountText.gameObject, Vector3.one * 1.5f, 0.2f).setEasePunch();
        }
    }

    public abstract void Activate();

    public void UsePowerUp()
    {
        if (remainingUses > 0)
        {
            remainingUses--;
            Debug.Log($"<color=cyan>{type} kullanıldı. Kalan hak: {remainingUses}</color>");
            UpdateVisuals(remainingUses);

            PlayClickAnimation();
            Activate();
        }
        else
        {
            Debug.Log($"<color=red>{type} hakkı bitti!</color>");
            PlayEmptyAnimation(); 
        }
    }

    protected virtual void PlayClickAnimation()
    {
        LeanTween.cancel(gameObject);
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
        transform.localScale = originalScale;

        float animDuration = 0.2f;

        // 1. Zıplama (Yukarı kalk ve geri in)
        LeanTween.moveLocalY(gameObject, originalPosition.y + 1f, animDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setLoopPingPong(1);

        // 2. Büyüme (Şiş ve geri in)
        LeanTween.scale(gameObject, originalScale * 1.3f, animDuration)
            .setEase(LeanTweenType.easeOutQuad)
            .setLoopPingPong(1);

        LeanTween.rotateAroundLocal(gameObject, Vector3.up, 360f, animDuration * 2f)
            .setEase(LeanTweenType.easeInOutCubic);
    }

    private void PlayEmptyAnimation()
    {
        LeanTween.cancel(gameObject);
        transform.localPosition = originalPosition;
        
        float moveAmount = 0.15f; 
        float speed = 0.12f;      

        LeanTween.moveLocalX(gameObject, originalPosition.x + moveAmount, speed)
            .setEase(LeanTweenType.easeInOutSine);

        LeanTween.moveLocalX(gameObject, originalPosition.x - moveAmount, speed * 2f)
            .setDelay(speed) 
            .setEase(LeanTweenType.easeInOutSine);

        LeanTween.moveLocalX(gameObject, originalPosition.x + moveAmount, speed * 2f)
            .setDelay(speed * 3f) 
            .setEase(LeanTweenType.easeInOutSine);

        LeanTween.moveLocalX(gameObject, originalPosition.x, speed)
            .setDelay(speed * 5f)
            .setEase(LeanTweenType.easeInOutSine);
    }
}