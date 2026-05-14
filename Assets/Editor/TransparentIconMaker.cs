using UnityEngine;
using UnityEditor;
using System.IO;

public class TransparentIconMaker
{
    // Çözünürlük ayarı (512x512 veya 256x256 yapabilirsin)
    private const int Resolution = 512; 

    [MenuItem("Tools/Take Transparent Icon (512x512)")]
    public static void TakeTransparentScreenshot()
    {
        // Sahnede MainCamera'yı bul
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Sahnede 'MainCamera' etiketli bir kamera bulunamadı moruk! Kamerana 'MainCamera' tag'i ver.");
            return;
        }

        // 1. Kameranın eski ayarlarını hafızaya al (İşimiz bitince bozmamak için)
        RenderTexture originalRT = cam.targetTexture;
        CameraClearFlags originalClearFlags = cam.clearFlags;
        Color originalColor = cam.backgroundColor;

        // 2. Kamerayı zorla ŞEFFAF SolidColor moda sok (Alpha=0)
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0, 0, 0, 0); // A=0 (Tam şeffaf)

        // 3. Jilet gibi 512x512 kare bir tuval oluştur (ARGB32 şeffaflık destekler)
        RenderTexture rt = new RenderTexture(Resolution, Resolution, 24, RenderTextureFormat.ARGB32);
        cam.targetTexture = rt;
        
        // 4. Tuvali boya (renderla)
        Texture2D screenShot = new Texture2D(Resolution, Resolution, TextureFormat.ARGB32, false);
        cam.Render();

        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, Resolution, Resolution), 0, 0);
        screenShot.Apply();

        // 5. Kamerayı eski haline getir (Ortalığı toparla)
        cam.targetTexture = originalRT;
        cam.clearFlags = originalClearFlags;
        cam.backgroundColor = originalColor;
        RenderTexture.active = null;
        Object.DestroyImmediate(rt); // Editör'de DestroyImmediate kullanılır

        // 6. PNG olarak kaydet (PNG alpha destekler)
        byte[] bytes = screenShot.EncodeToPNG();
        string folderPath = Application.dataPath + "/TransparentIcons";
        
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string fileName = "TransparentIcon_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string fullPath = folderPath + "/" + fileName;

        File.WriteAllBytes(fullPath, bytes);
        Debug.Log($"<color=green>📸 Jilet gibi ŞEFFAF Icon başarıyla kaydedildi: {fullPath}</color>");

        // Unity'yi yenile (dosyayı Asset klasöründe görsün)
        AssetDatabase.Refresh();
    }
}