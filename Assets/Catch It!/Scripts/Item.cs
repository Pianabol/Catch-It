using UnityEngine;

public class Item : MonoBehaviour
{
   [Header(" Elements")]
   [SerializeField] private Rigidbody rb;
   [SerializeField] private Collider col;
   
   private Renderer[] allRenderers;

    void Awake()
    {
        allRenderers = GetComponentsInChildren<Renderer>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableShadows()
    {
        foreach(Renderer r in allRenderers)
        {
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }

    public void DisablePhysics()
    {
        rb.isKinematic = true;
        col.enabled = false;
    }

}
