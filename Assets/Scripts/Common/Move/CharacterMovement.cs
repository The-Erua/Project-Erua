using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float dashSpeedMultiplier = 2.0f;
    public float jumpHeight = 2.0f;
    public Camera mainCamera;
    private Rigidbody rb;
    
    private bool isDashing = false;
    private bool isGrounded = false;
    private Vector3 moveDirection = Vector3.zero;
    private Animator animator;
    private bool isWalking;

    public static class AnimationConsts
    {
        public static readonly int IsWalk = Animator.StringToHash("isWalk");
        public static readonly int IsRunning = Animator.StringToHash("isRunning");
        public static readonly int DWARF_WALK = Animator.StringToHash("Dwarf Walk");
        // private static readonly int IsWalk = Animator.StringToHash("isWalk");
        // private static readonly int IsWalk = Animator.StringToHash("isWalk");
        // private static readonly int IsWalk = Animator.StringToHash("isWalk");
        // private static readonly int IsWalk = Animator.StringToHash("isWalk");
        // private static readonly int IsWalk = Animator.StringToHash("isWalk");
    }

    public class AnimationManager
    {
        private static AnimationManager instance;

        // 프로퍼티
        public static AnimationManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new AnimationManager();
                
                return instance;
            }
        }

        public void SetBool(Animator animator, int id, bool value)
        {
            animator.SetBool(id, value);
        }

        public void Play(Animator animator, int stateHash, int layer)
        {
            animator.enabled = true;
            animator.Play(stateHash,layer);
        }
        
        public void Stop(Animator animator)
        {
            animator.enabled = false;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 카메라의 정면과 오른쪽 방향을 기준으로 입력을 계산
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        forward.y = 0; // 카메라의 상하 방향은 무시
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        moveDirection = (moveHorizontal * right + moveVertical * forward).normalized;

        if (moveDirection.magnitude > 0.3f && isWalking == false)
        {
            AnimationManager.Instance.Play(animator, AnimationConsts.DWARF_WALK, 0);
            isWalking = true;
        }
        else
        {
            AnimationManager.Instance.Stop(animator);
            isWalking = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartDash();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopDash();
        }
    }

    void FixedUpdate()
    {
        MoveCharacter();
    }

    void MoveCharacter()
    {
        if (isDashing)
        {
            var targetPos = rb.position + moveDirection * (moveSpeed * dashSpeedMultiplier) * Time.fixedDeltaTime;
            targetPos.y = 0;
            rb.MovePosition(targetPos);
        }
        else
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            AnimationManager.Instance.Stop(animator);
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
        isGrounded = false;
    }

    void StartDash()
    {
        isDashing = true;
    }

    void StopDash()
    {
        isDashing = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

    
    }
}
