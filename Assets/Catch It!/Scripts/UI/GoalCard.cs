using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalCard : MonoBehaviour
{
    [Header(" Elemanlar ")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject checkMark;
    [SerializeField] private GameObject backFace;

    public void Configure(int initialAmount, Sprite icon)
    {
        LeanTween.cancel(gameObject); // Varsa eski animasyonları temizle
        transform.localScale = Vector3.one;
        transform.localEulerAngles = Vector3.zero;  
        
        iconImage.gameObject.SetActive(true);
        amountText.gameObject.SetActive(true);
        checkMark.SetActive(false);
        backFace.SetActive(false);

        amountText.text = initialAmount.ToString();
        iconImage.sprite = icon;
    }

    public void UpdateAmount(int newAmount)
    {
        amountText.text = newAmount.ToString();
        Bump();
    }

    private void Bump()
    {
        LeanTween.cancel(gameObject);
        
        transform.localScale = Vector3.one;

        LeanTween.scale(gameObject, Vector3.one * 1.15f, 0.15f)
            .setEase(LeanTweenType.easeOutQuad)
            .setLoopPingPong(1);
    }

    public void Complete()
    {
        LeanTween.cancel(gameObject);
        transform.localScale = Vector3.one;

        amountText.gameObject.SetActive(false);
        checkMark.SetActive(true);

        // Zamanlar 
        float delayBeforeSpin = 0.5f; // Tik çıktıktan sonra bekleme süresi
        float spinDuration = 0.8f;    // 540 derecelik dönüşün toplam süresi
        float diminishDuration = 0.3f;// Küçülme (yok olma) süresi
        
        float showBackFaceTime = delayBeforeSpin + (spinDuration * (450f / 540f)); 
        
        // Küçülmeye başlama anı: Dönüşün bitmesine tam 'diminishDuration' kala
        float diminishStartTime = delayBeforeSpin + spinDuration - diminishDuration;

        // 3. 540 Derece Döndürme Şovu (Y ekseninde)
        LeanTween.rotateAroundLocal(gameObject, Vector3.up, 540f, spinDuration)
            .setDelay(delayBeforeSpin)
            .setEase(LeanTweenType.easeInOutCubic); // Yumuşak başlayıp yumuşak bitsin

        // 4. Tam 450. Derecede (Kart bize tam yan dönerken) Arka Yüzü Aç, Önü Kapat
        LeanTween.delayedCall(gameObject, showBackFaceTime, () => {
            backFace.SetActive(true);
            iconImage.gameObject.SetActive(false);
            checkMark.SetActive(false);
        });

        // 5. Dönüşün son anlarında (Scale Down) Küçülerek Yok Ol
        LeanTween.scale(gameObject, Vector3.zero, diminishDuration)
            .setDelay(diminishStartTime)
            .setEase(LeanTweenType.easeInBack) // İçine çökerek tatlı bir küçülme
            .setOnComplete(() => {
                // Tamamen küçülünce objeyi kapat. 
                gameObject.SetActive(false); 
            });
    }
}
