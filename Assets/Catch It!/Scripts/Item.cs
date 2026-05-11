using UnityEngine;

public class Item : MonoBehaviour
{
    public int PrefabID { get; set; } 
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

    
    
    [Header(" Despawn Settings ")]
    [SerializeField] private float offScreenXThreshold = -20f; 

    private Renderer[] allRenderers;
    private int currentTweenId; 
    private int turnTweenId; // Dönüş animasyonu iptali için ID

    void Awake()
    {
        allRenderers = GetComponentsInChildren<Renderer>();
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

                
                // Üst duvara çarptıysa eksi (aşağı), alt duvara çarptıysa artı (yukarı) değer verir.
                float wallDirectionZ = Mathf.Sign(contact.normal.z);
                
                // Merkeze doğru rastgele ama GÜÇLÜ bir sekme hızı belirliyoruz
                float enforcedZSpeed = Random.Range(minZBounceForce, maxZBounceForce);
                
                reflectedVelocity.z = wallDirectionZ * enforcedZSpeed;
                // ---------------------------------------------------

                reflectedVelocity *= bounceSpeedMultiplier;

                if (reflectedVelocity.magnitude > maxBounceSpeed)
                {
                    reflectedVelocity = reflectedVelocity.normalized * maxBounceSpeed;
                }

                rb.linearVelocity = reflectedVelocity;

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