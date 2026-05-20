using UnityEngine;

public class FreezeGun : PowerUp
{
    private Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.localPosition;
    }

    public override void Activate()
    {
        Debug.Log("<color=cyan>Freeze Gun: Zaman donduruldu, Timer buz tuttu!</color>");
    }

    protected override void PlayClickAnimation()
    {
        LeanTween.cancel(gameObject);
        transform.localPosition = originalPos;

        LeanTween.moveLocalY(gameObject, originalPos.y + 1.5f, 0.2f).setEaseOutQuad().setOnComplete(() =>
        {
            Debug.Log("Silah Ateş Etti! PEW PEW!");
    
            LeanTween.moveLocalZ(gameObject, transform.localPosition.z - 0.5f, 0.1f).setLoopPingPong(1);

            LeanTween.delayedCall(0.5f, () => 
            {
                LeanTween.moveLocalY(gameObject, originalPos.y, 0.2f).setEaseInQuad();
            });
        });
    }
}