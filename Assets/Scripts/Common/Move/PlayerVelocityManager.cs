using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using EditorLog;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerVelocityManager : MonoBehaviourSingleton<PlayerVelocityManager>
{
    public Camera mainCamera;
    public float timescaler = 1;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;
    private bool isGrounded = false;
    public float jumpForce = 0;
    private bool isSlow;    
    public float moveSpeed = 5.0f;
    private bool shouldJump;
    private float deltaVelocity;
    [SerializeField] private float rotationSpeedOffset = 10f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveDirection = GetMoveDirection().normalized * moveSpeed * timescaler;
        if(moveDirection.magnitude < 0.3f)
            moveDirection = Vector3.zero;
        HandleManager.Instance.SetMoveDir(moveDirection);
        
    }

    private void FixedUpdate()
    {
        if (shouldJump)
        {
            var backMove = (rb.velocity * Time.fixedDeltaTime * (1 - timescaler));
            rb.position -= backMove;
        }
    }

    private Vector3 GetMoveDirection()
    {
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        forward.y = 0; // 카메라의 상하 방향 무시
        right.y = 0;

        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");
        Vector3 desiredDirection = (moveVertical * forward + moveHorizontal * right).normalized;

        if (desiredDirection.magnitude < 0.3f)
            return Vector3.zero;
        
        Quaternion targetRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * rotationSpeedOffset);

        return desiredDirection;
    }

    public void SetJump(bool shouldJump)
    {
        this.shouldJump = shouldJump;
        if(shouldJump)
           rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            HandleManager.Instance.SetGround(isGrounded);
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            HandleManager.Instance.SetGround(isGrounded);
        }
    }

    public void ApplySlowMotion(float timeFactor)
    {
        timescaler = timeFactor;
    }
    public void ResetSpeed(Vector3 originalVelocity)
    {
        timescaler = 1;
    }
    public void DoMove()
    {
        // X와 Z축은 새로운 이동 방향으로 설정하고, Y축은 현재 Y축 속도(중력 영향)를 유지합니다.
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
    }
}
