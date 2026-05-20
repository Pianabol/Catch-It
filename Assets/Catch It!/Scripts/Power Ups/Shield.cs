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
        Debug.Log($"<color=cyan>Shield Aktif! {duration} saniye boyunca ceza almayacaksın!</color>");

        // TODO: İleride ekranın kenarlarına mavi bir parlama veya karakterin etrafına hale efekti ekleyebilirsin.

        LeanTween.delayedCall(gameObject, duration, () =>
        {
            IsActive = false;
            Debug.Log("<color=orange>Shield süresi bitti. Artık korumasızsın!</color>");
        });
    }
}