using System;
using System.Collections;
using System.Collections.Generic;
using EditorLog;
using UnityEngine;

public class HandleManager: MonoBehaviourSingleton<HandleManager>
{
    public IMovementState state;
    public Rigidbody playerRb;
    public Animator animator;
    public AnimationState prevAnimState;
    internal float impulseVal = 3.78f;
    public Vector3 moveDir;
    internal float speedMultiplier = 3f;

    public void SetPlayer(Rigidbody playerRb, Animator animator)
    {
        this.playerRb = playerRb;
        this.animator = animator;
        AnimationStateTableManager.Instance.InitAllAnimationState();
        state = new IdleState();
    }

    private void Update()
    {
        if (!playerRb || !animator)
            return;
        try
        {
            state.HandleInput(this);
            state.TryPlayAnimation(this);
        }
        catch (Exception e)
        {
            EditorDebug.LogError(e);
        }
    }

    private void LateUpdate()
    {
        if (!playerRb || !animator)
            return;
        try
        {
            state.ExecuteMovement(playerRb);
        } 
        catch (Exception e)
        {
            EditorDebug.LogError(e);
        }
    }

    public void ChangeMovementState(IMovementState newState) => state = newState;
    public void ChangeAnimState(AnimationState newAnimState) => prevAnimState = newAnimState;
    public void SetMoveDir(Vector3 moveDir)
    {
        this.moveDir = moveDir;
    }

}
