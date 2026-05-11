using UnityEngine;

public class Item : MonoBehaviour
{
    public int PrefabID { get; set; } 
    
    private static readonly int Color1ID = Shader.PropertyToID("_Color_1");
    private static readonly int Color2ID = Shader.PropertyToID("_Color_2");
    private static readonly int Color3ID = Shader.PropertyToID("_Color_3_Overlay");

    // Performans dostu renk değiştirme bloğumuz
    private MaterialPropertyBlock mpb;
    private float currentSpinAngle; // Burgu dönüşünü aklında tutması için
    private Vector3 visualForwardDirection; // Görsel olarak baktığı yön (Yumuşak dönüş için)
    private bool isDead; // Çoklu çarpışma bug'ını (fazla patlamayı) engellemek için

    [Header(" Elements")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;



    [Header(" Movement & Panic (Aşama 1)")]
    [SerializeField] private float normalSpeed = 4f; 
    [SerializeField] private float panicSpeedMultiplier = 2.5f; 
    [SerializeField] private float panicDuration = 1.5f; 
    [SerializeField] private float spinSpeed = 90f; 

    
    
    [Header(" Wall Bounce Settings (Aşama 2)")]
    [SerializeField, Range(0f, 1f)] private float wallExplosionProbability = 0.15f; // Inspector'dan ayarlanabilir patlama ihtimali
    [SerializeField] private float bounceSpeedMultiplier = 1.3f; 
    [SerializeField] private float maxBounceSpeed = 15f; 
    [SerializeField] private float turnSmoothDuration = 0.3f; // Duvara çarpınca kafasını ne kadar sürede çevirsin?
    [SerializeField] private float minZBounceForce = 3f; // Duvara çarpınca merkeze doğru minimum fırlama gücü
    [SerializeField] private float maxZBounceForce = 6f; // Maksimum fırlama gücü


    [Header(" Health & Rage Settings (Aşama 3)")]
    [SerializeField] private int maxHealth = 1;  
    [SerializeField] private float rageDashMultiplier = 3f;  
    private int currentHealth;

    
    
    [Header(" Despawn Settings ")]
    [SerializeField] private float offScreenXThreshold = -20f; 

    private Renderer[] allRenderers;
    private int currentTweenId; 
    private int turnTweenId; // Dönüş animasyonu iptali için ID

    void Awake()
    {
        allRenderers = GetComponentsInChildren<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    void Update()
    {
        // Eğer virüs hareket ediyorsa
        if (rb.linearVelocity.sqrMagnitude > 0.1f)
        {
            currentSpinAngle += spinSpeed * Time.deltaTime;

            // Artık direkt fiziksel hıza değil, LeanTween ile yumuşatılmış görsel yöne bakıyor
            if (visualForwardDirection != Vector3.zero)
            {
                Quaternion lookDirection = Quaternion.LookRotation(visualForwardDirection);
                transform.rotation = lookDirection * Quaternion.Euler(0, 0, currentSpinAngle);
            }
        }

        if (transform.position.x < offScreenXThreshold)
        {
            ReturnToPool();
        }
    }

    public void Initialize()
    {
        isDead = false; // Havuzdan çıkınca hayata geri döndür
        rb.isKinematic = false;
        col.enabled = true;
        rb.linearVelocity = Vector3.zero; 
        rb.angularVelocity = Vector3.zero;
        
        LeanTween.cancel(currentTweenId);
        LeanTween.cancel(turnTweenId);

        currentSpinAngle = 0f; 
        currentHealth = maxHealth;

        PushWithPanic();
        
    
        visualForwardDirection = rb.linearVelocity.normalized; 
    }

    private void PushWithPanic()
    {
        float randomZDirection = Random.Range(-0.8f, 0.8f);
        Vector3 moveDirection = new Vector3(-1, 0, randomZDirection).normalized;

        float currentPanicSpeed = normalSpeed * panicSpeedMultiplier;
        rb.linearVelocity = moveDirection * currentPanicSpeed;

        currentTweenId = LeanTween.value(gameObject, currentPanicSpeed, normalSpeed, panicDuration)
            .setEase(LeanTweenType.easeOutCubic) 
            .setOnUpdate((float currentSpeed) =>
            {
                if (gameObject.activeInHierarchy && !rb.isKinematic)
                {
                    rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
                }
            }).id;
    }

    public virtual bool TakeDamage()
    {
        currentHealth--;

        if (currentHealth <= 0)
        {
            return true; // Virüs öldü!
        }
        else
        {
            // Virüs ölmedi, ÖFKE MODU! (Dash atıyor)
            // Mevcut hızını aniden rageDashMultiplier ile çarpıyoruz
            rb.linearVelocity = rb.linearVelocity.normalized * (rb.linearVelocity.magnitude * rageDashMultiplier);
            
            // Eğer istersen buraya sonradan Viyaklama Sesi (SFX) ekleyeceğiz
            
            ApplyHitEffect();

            return false; // Virüs hala hayatta!
        }
    }

    private void DamageFlash()
    {
        // 1. Patlama rengimiz (HDR Beyaz)
        Color flashColor = Color.white * 5f; 

        // 2. Virüsün kendi orijinal renklerini materyalinden okuyalım (Siyaha dönmesin diye)
        // İlk renderer'ın materyaline bakmamız yeterli, hepsi aynı zaten
        Material mat = allRenderers[0].sharedMaterial;
        Color origColor1 = mat.GetColor(Color1ID);
        Color origColor2 = mat.GetColor(Color2ID);
        Color origColor3 = mat.GetColor(Color3ID);

        // 3. 0.1 saniyede beyaza git ve geri dön
        LeanTween.value(gameObject, 0f, 1f, 0.1f)
            .setLoopPingPong(1)
            .setOnUpdate((float t) =>
            {
                // Orijinal renklerden -> Beyaza doğru yumuşak geçiş (Lerp)
                Color lerped1 = Color.Lerp(origColor1, flashColor, t);
                Color lerped2 = Color.Lerp(origColor2, flashColor, t);
                Color lerped3 = Color.Lerp(origColor3, flashColor, t);

                foreach (Renderer r in allRenderers)
                {
                    r.GetPropertyBlock(mpb);
                    // Senin shader'ındaki 3 rengi de aynı anda patlatıyoruz!
                    mpb.SetColor(Color1ID, lerped1);
                    mpb.SetColor(Color2ID, lerped2);
                    mpb.SetColor(Color3ID, lerped3);
                    r.SetPropertyBlock(mpb);
                }
            }).setOnComplete(() => {
                // SİHİRLİ DOKUNUŞ: Animasyon bitince PropertyBlock'u temizle (null).
                // Bu sayede virüs anında Inspector'daki o kendi orijinal kusursuz haline sıfır hatayla geri döner.
                foreach (Renderer r in allRenderers)
                {
                    r.SetPropertyBlock(null); 
                }
            });
    }

    private void ApplyHitEffect()
    {
        DamageFlash();
        
        // 1. FİZİKSEL SIÇRAMA VE YÖN DEĞİŞİMİ
        float randomAngle = Random.Range(-25f, 25f); 
        Vector3 currentDir = rb.linearVelocity.normalized;
        Vector3 newDir = Quaternion.Euler(0, randomAngle, 0) * currentDir;

        // Ani sıçrama hızı
        rb.linearVelocity = newDir * (normalSpeed * rageDashMultiplier);

        // --- YENİ EKLENEN: YUMUŞAK YÖN DÖNÜŞÜ (SMOOTH TURN) ---
        // Duvara çarptığındaki aynı yumuşak çevirme işlemini burada da tetikliyoruz
        LeanTween.cancel(turnTweenId);
        turnTweenId = LeanTween.value(gameObject, visualForwardDirection, newDir.normalized, turnSmoothDuration)
            .setEase(LeanTweenType.easeOutSine) 
            .setOnUpdate((Vector3 interpolatedDir) => 
            {
                visualForwardDirection = interpolatedDir.normalized;
            }).id;
        // --------------------------------------------------------

        // 2. GÖRSEL SCALING
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = originalScale * 1.25f;

        LeanTween.scale(gameObject, targetScale, 0.2f)
            .setEase(LeanTweenType.easeOutBack) 
            .setOnComplete(() => {
                LeanTween.scale(gameObject, originalScale, 0.2f).setEase(LeanTweenType.easeInSine);
            });
    }

    public void DisablePhysics()
    {
        rb.isKinematic = true;
        col.enabled = false;
        rb.linearVelocity = Vector3.zero;
        
        LeanTween.cancel(currentTweenId); 
        LeanTween.cancel(turnTweenId); // Obje kapanınca dönüşü de iptal et
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Wall"))
        {
            if (Random.value <= wallExplosionProbability)
            {
                isDead = true; 
                InputManager.itemClicked?.Invoke(this); 
            }
            else
            {
                ContactPoint contact = collision.contacts[0];
                Vector3 reflectedVelocity = Vector3.Reflect(rb.linearVelocity, contact.normal);
                reflectedVelocity.y = 0; 

                // 1. AÇIYI KISMA (Dikey sekmeyi törpüleme):
                // Z eksenindeki yansıma hızını %50 oranında düşürüyoruz.
                // Böylece dikine (ping-pong gibi) sekmek yerine daha yatay ve hedefe yönelik bir açı kazanır.
                reflectedVelocity.z *= 0.5f; 

                // 2. KESİN SIKIŞMA ÖNLEYİCİ (-X Yönü Garantisi):
                // Eğer virüs sola doğru yeterince hızlı gitmiyorsa (veya sağa doğru sekmeye çalışıyorsa)
                if (reflectedVelocity.x > -5f)
                {
                    // ŞLAK! Onu zorla ve rastgele bir güçle sola (-X yönüne) doğru ateşle.
                    // (Değerleri oyunun hızına göre 5f-8f veya 6f-10f yapabilirsin)
                    reflectedVelocity.x = -Random.Range(5f, 8f);
                }

                // 3. PARALEL KAYMA ÖNLEYİCİ (Z için minimum güç):
                // Çarptığı duvarın yönünü bul (Üst duvar eksi, alt duvar artı verir)
                float wallDirectionZ = Mathf.Sign(contact.normal.z);
                
                // Eğer Z hızı çok düşüp duvara paralel kaymaya çalışırsa, onu hafifçe duvardan it
                if (Mathf.Abs(reflectedVelocity.z) < 2f) 
                {
                    reflectedVelocity.z = wallDirectionZ * Random.Range(2f, 4f);
                }

                // Hızlandır ve limiti aşarsa kelepçele
                reflectedVelocity *= bounceSpeedMultiplier;
                if (reflectedVelocity.magnitude > maxBounceSpeed)
                {
                    reflectedVelocity = reflectedVelocity.normalized * maxBounceSpeed;
                }

                rb.linearVelocity = reflectedVelocity;

                // Dönüş Animasyonu (Aynı)
                LeanTween.cancel(turnTweenId);
                turnTweenId = LeanTween.value(gameObject, visualForwardDirection, reflectedVelocity.normalized, turnSmoothDuration)
                    .setEase(LeanTweenType.easeOutSine) 
                    .setOnUpdate((Vector3 interpolatedDir) => 
                    {
                        visualForwardDirection = interpolatedDir.normalized;
                    }).id;
            }
        }
    }

    public void ReturnToPool()
    {
        DisablePhysics(); 
        PoolManager.Instance.ReturnItem(this); 
    }
}