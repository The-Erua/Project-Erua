using UnityEngine;
 
[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviourSingleton<CustomGravity>
{
    // Gravity Scale editable on the inspector
    // providing a gravity scale per object
 
    public float gravityScale = 1.0f;
    public float timeScale { get; set; }
 
    // Global Gravity doesn't appear in the inspector. Modify it here in the code
    // (or via scripting) to define a different default gravity for all objects.
 
    public static float globalGravity = -9.81f;
 
    public float Gravity => gravityScale * globalGravity;
    
    Rigidbody m_rb;
 
    void OnEnable ()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.useGravity = false;
    }
    
    void FixedUpdate ()
    {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        // 중력을 velocity에 직접 적용합니다.
        m_rb.velocity += gravity * Time.fixedDeltaTime * timeScale;
    }

}