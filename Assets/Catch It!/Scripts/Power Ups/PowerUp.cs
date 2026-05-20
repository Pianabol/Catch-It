using UnityEngine;
using TMPro; // Eğer kalan hakkı UI'da göstereceksen

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
    [SerializeField] protected int remainingUses = 3; // Başlangıçta 3 hak verelim mesela
    
    // Abstract metot: Bunu miras alan her script, içini kendi kuralına göre doldurmak ZORUNDA!
    public abstract void Activate();

    // Ortak bir kullanım (tıklanma) metodu
    public void UsePowerUp()
    {
        if (remainingUses > 0)
        {
            remainingUses--;
            Debug.Log($"<color=cyan>{type} kullanıldı. Kalan hak: {remainingUses}</color>");
            
            // Kendi görsel/animasyon efektini (LeanTween) buraya ekleyebilirsin
            PlayClickAnimation();

            // Gerçek gücü (FirstAid, Freeze vs.) ateşle
            Activate();
        }
        else
        {
            Debug.Log($"<color=red>{type} hakkı bitti!</color>");
            // Tıklanamaz animasyonu (örn: sağa sola titreme) oynatılabilir.
        }
    }

    private void PlayClickAnimation()
    {
        /* 
        // Örnek basit bir basılma efekti
        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.one;
        LeanTween.scale(gameObject, Vector3.one * 0.8f, 0.1f).setLoopPingPong(1);
        */
        
    }
}