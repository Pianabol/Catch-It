using UnityEngine;

public class Shield : PowerUp
{
    [Header(" Shield Settings ")]
    [Tooltip("Kalkanın kaç saniye aktif kalacağı")]
    [SerializeField] private float duration = 5f;
    public static bool IsActive { get; private set; }

    public override void Activate()
    {
        if (IsActive)
        {
            LeanTween.cancel(gameObject); 
        }

        IsActive = true;


        LeanTween.delayedCall(gameObject, duration, () =>
        {
            IsActive = false;
        });
    }
}