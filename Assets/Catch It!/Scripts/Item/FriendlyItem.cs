using UnityEngine;

// DİKKAT: MonoBehaviour yerine Item'dan miras alıyoruz!
// Artık Item.cs içindeki bütün hareket, sekme ve havuz yeteneklerine doğuştan sahip.
public class FriendlyItem : Item 
{
    [Header(" Friendly Settings (Aşama 4) ")]
    public int penaltyDamage = 1;  

    public override bool TakeDamage()
    {
        // İleride burayı Player'ın canını düşürecek şekilde güncelleyeceğiz
        Debug.LogWarning("⚠️ DİKKAT! DOST HAPA TIKLADIN! CEZA ALDIN!");

        // Dost objeler (Haplar) tıklandığı an tek seferde patlasın ve havuza dönsün
        return true; 
    }
}