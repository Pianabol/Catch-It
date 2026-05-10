using UnityEngine;

public class Item : MonoBehaviour
{
    // PoolManager'ın atadığı kimlik numarası
    public int PrefabID { get; set; } 

    [Header(" Elements")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;

    [Header(" Movement")]
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float maxSpeed = 7f;

    private Renderer[] allRenderers;

    void Awake()
    {
        allRenderers = GetComponentsInChildren<Renderer>();
    }
    public void Initialize()
    {
        // 1. Kapatılmış fizikleri geri aç
        rb.isKinematic = false;
        col.enabled = true;

        // 2. Havuzdan kalan eski hızını/dönüşünü tamamen sil
        rb.linearVelocity = Vector3.zero; 
        rb.angularVelocity = Vector3.zero;
        //Deneme 
        transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

        
        PushSlightly();
    }

    private void PushSlightly()
    {
        float randomSpeed = Random.Range(minSpeed, maxSpeed);
        float randomZDirection = Random.Range(-5f, 5f); // Z ekseninde (yukarı/aşağı) hafif sapma

        // -X yönünde (sağdan sola) itiyoruz
        rb.linearVelocity = new Vector3(-randomSpeed, 0, randomZDirection);
    }

    public void DisablePhysics()
    {
        rb.isKinematic = true;
        col.enabled = false;
        rb.linearVelocity = Vector3.zero;
    }

    public void ReturnToPool()
    {
        PoolManager.Instance.ReturnItem(this);
    }
}