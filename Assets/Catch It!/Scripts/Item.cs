using UnityEngine;

public class Item : MonoBehaviour
{
   [Header(" Elements")]
   [SerializeField] private Rigidbody rb;
   [SerializeField] private Collider col;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisableShadows()
    {
    
    }

    public void DisablePhysics()
    {
        rb.isKinematic = true;
        col.enabled = false;
    }
}
