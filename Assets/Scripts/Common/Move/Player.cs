using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    
    private bool isDashing = false;
    private Animator animator;
    private bool isWalking;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        HandleManager.Instance.SetPlayer(rb, animator);
    }
}
