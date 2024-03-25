using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using EditorLog;
using UnityEngine;
using UnityEngine.Serialization;
using Color = UnityEngine.Color;
using Debug = UnityEngine.Debug;

public enum CurrentJumpState
{
    NonInitialized,
    ReadyToJump, // 점프 직전
    OnAir, // 상승 중
    Grounded // 착지
}

public class PlayerMovementManager : MonoBehaviourSingleton<PlayerMovementManager>
{
   
    public float timescaler = 1;

    private CurrentJumpState _currentJumpState;

    public Vector3 DebugRbVelocity;
    public CurrentJumpState CurrentJumpState
    {
        get
        {
            return _currentJumpState;
        }
        set { _currentJumpState = value; }
    }
    private Camera mainCamera;
    private Vector3 moveDirection = Vector3.zero;
    private Rigidbody rb;
    private bool isSlow;    
    private bool shouldJump;
    private float deltaVelocity;
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpForce = 0;
    [SerializeField] private float rotationSpeedOffset = 10f;
    [SerializeField] private float runSpeed;
    private ContactPoint prevContact;
    private bool havePrevContact;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        moveDirection = GetMoveDirection().normalized * moveSpeed * timescaler;
        if(moveDirection.magnitude < 0.3f)
            moveDirection = Vector3.zero;
        HandleManager.Instance.SetMoveDir(moveDirection);
        
        if(CheckJumpState(CurrentJumpState.ReadyToJump))
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR
        DebugRbVelocity = rb.velocity;
#endif
        if (CheckJumpState(CurrentJumpState.OnAir))
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
        rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.deltaTime * rotationSpeedOffset * timescaler);

        return desiredDirection;
    }
    
    // 이 클래스에서만 사용하는 체크 로직
    private bool CheckJumpState(CurrentJumpState targetState)
    {
        return targetState == _currentJumpState;
    }

    // 다른 클래스에서만 사용하는 체크 로직
    public bool IsCurrentJumpState(CurrentJumpState targetState)
    {
        return targetState == CurrentJumpState;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (IsWall(collision.GetContact(0)))
                return;
            if(CheckJumpState(CurrentJumpState.NonInitialized))
                CurrentJumpState = CurrentJumpState.Grounded;
            else if(CheckJumpState(CurrentJumpState.OnAir))
                CurrentJumpState = CurrentJumpState.Grounded;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (havePrevContact && IsWall(prevContact))
            {
                prevContact = default;
                havePrevContact = false;
                return;
            }

            if(CheckJumpState(CurrentJumpState.ReadyToJump))
                CurrentJumpState = CurrentJumpState.OnAir;
            else if (CheckJumpState(CurrentJumpState.Grounded))
                CurrentJumpState = CurrentJumpState.OnAir;
        }
    }

    private bool IsWall(ContactPoint contact)
    {
        // 충돌 지점의 노말 벡터를 얻습니다.
        Debug.DrawRay(contact.point, contact.normal * 10, Color.red);

        // 벡터 간의 각도를 계산합니다. 여기서는 노말 벡터와 벡터의 업 벡터(0,1,0) 간의 각도를 사용합니다.
        float angle = Vector3.Angle(contact.normal, Vector3.up);

        // 각도가 30도 이상이면 벽으로 간주합니다.
        if (angle > 30)
        {
            Debug.Log("This is considered a wall.");
            prevContact = contact;
            havePrevContact = true;
            // 필요한 경우 여기에서 false를 반환하거나 관련 로직을 처리합니다.
            return true; // 현재 메서드에서 더 이상의 처리를 중단합니다.
        }

        return false;
    }
    public void ApplySlowMotion(float timeFactor)
    {
        timescaler = timeFactor;
    }
    public void ResetSpeed()
    {
        timescaler = 1;
    }
    public void DoMove()
    {
        // X와 Z축은 새로운 이동 방향으로 설정하고, Y축은 현재 Y축 속도(중력 영향)를 유지합니다.
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
    }
    
    public void DoRun()
    {
        moveDirection *= runSpeed;
        // X와 Z축은 새로운 이동 방향으로 설정하고, Y축은 현재 Y축 속도(중력 영향)를 유지합니다.
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
    }
}
