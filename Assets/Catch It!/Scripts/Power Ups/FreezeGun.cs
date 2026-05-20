using UnityEngine;
using NaughtyAttributes;

public class FreezeGun : PowerUp
{
    [Header(" Aim Settings ")]
    [Tooltip("Silahın ateş etmek için gideceği pozisyon")]
    [SerializeField] private Vector3 aimPosition = new Vector3(0f, -0.18f, -0.4f);
    
    [Tooltip("Silahın ateş etmek için alacağı açı")]
    [SerializeField] private Vector3 aimRotation = new Vector3(-35.233f, -90f, 180f);

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
        Debug.Log("<color=cyan>PEW! Freeze Gun Ateş Etti!</color>");

        if (TimerManager.Instance != null)
        {
            TimerManager.Instance.FreezeTimer(5f);
        }
    }
}