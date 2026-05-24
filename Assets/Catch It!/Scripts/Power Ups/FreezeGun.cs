using UnityEngine;
using NaughtyAttributes;

public class FreezeGun : PowerUp
{
    [Header(" Aim Settings ")]
    [Tooltip("Silahın ateş etmek için gideceği pozisyon")]
    [SerializeField] private Vector3 aimPosition = new Vector3(0f, -0.18f, -0.4f);
    
    [Tooltip("Silahın ateş etmek için alacağı açı")]
    [SerializeField] private Vector3 aimRotation = new Vector3(-35.233f, -90f, 180f);

    [Header(" VFX Settings ")]
    [Tooltip("Silahın ucundaki boş GameObject (Shooting Point)")]
    [SerializeField] private Transform shootingPoint;
    
    [Tooltip("Patlayacak o şekil buz efektinin Prefab'ı")]
    [SerializeField] private GameObject freezeEffectPrefab;
    public override void Activate()
    {
        // simdilik bos
    }
    protected override void PlayClickAnimation()
    {
        LeanTween.cancel(gameObject);
    
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;

        float aimDuration = 0.3f; 


        LeanTween.moveLocal(gameObject, aimPosition, aimDuration).setEase(LeanTweenType.easeOutQuad);
        LeanTween.rotateLocal(gameObject, aimRotation, aimDuration).setEase(LeanTweenType.easeOutQuad).setOnComplete(() =>
        {
            // Recoil
            LeanTween.moveLocalZ(gameObject, aimPosition.z + 0.15f, 0.1f).setLoopPingPong(1).setOnComplete(() => 
            {
                FireFreezeBeam();

                LeanTween.delayedCall(0.4f, () => 
                {
                    LeanTween.moveLocal(gameObject, originalPosition, aimDuration).setEase(LeanTweenType.easeInOutSine);
                    LeanTween.rotateLocal(gameObject, originalRotation.eulerAngles, aimDuration).setEase(LeanTweenType.easeInOutSine);
                });
            });
        });
    }

    private void FireFreezeBeam()
    {
        if (shootingPoint != null && freezeEffectPrefab != null)
        {
            // Efekti tam namlunun ucunda, namlunun baktığı açıya göre oluştur
            GameObject vfx = Instantiate(freezeEffectPrefab, shootingPoint.position, shootingPoint.rotation);
            
            // Sahneyi çöplüğe çevirmemek için efekti 2 saniye sonra otomatik yok et
            // (Eğer efektin kendi süresi daha uzun/kısaysa buradaki 2f değerini ona göre değiştirebilirsin)
            Destroy(vfx, 2f); 
        }
        else
        {
            //Debug
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayPowerUpSound("freeze");
        }
        
        if (TimerManager.Instance != null)
        {
            TimerManager.Instance.FreezeTimer(5f);
        }
    }
}