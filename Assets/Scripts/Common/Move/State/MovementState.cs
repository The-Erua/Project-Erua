using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementState
{
    void HandleInput(HandleManager handleManager);
    void ExecuteMovement(Rigidbody playerRb);
    void TryPlayAnimation(HandleManager animator);
}

public class JumpState : IMovementState
{
    public void HandleInput(HandleManager handleManager)
    {
        if (AnimationUtil.IsAnimationEnd(handleManager.animator))
        {
            AnimationStateTableManager.Instance.TryGetAnimationState(out AnimationState newState, AnimationHash.IDLE);
            handleManager.ChangeMovementState(new IdleState());
        }
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
        if(Input.GetKeyDown(KeyCode.Space))
            playerRb.AddForce(Vector3.up * HandleManager.Instance.impulseVal, ForceMode.Impulse);
    }

    public void TryPlayAnimation(HandleManager mgr)
    {
        if(AnimationUtil.HandleAnimation(mgr.animator, mgr.prevAnimState, AnimationHash.JUMP))
        {
            AnimationStateTableManager.Instance.TryGetAnimationState(out AnimationState newState, AnimationHash.JUMP);
            mgr.ChangeAnimState(newState);
        }
    }
}

public class DashState : IMovementState
{
    public void HandleInput(HandleManager handleManager)
    {
        
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
    }

    public void TryPlayAnimation(HandleManager mgr)
    {
        if(AnimationUtil.HandleAnimation(mgr.animator, mgr.prevAnimState, AnimationHash.DASH))
        {
            AnimationStateTableManager.Instance.TryGetAnimationState(out AnimationState newState, AnimationHash.DASH);
            mgr.ChangeAnimState(newState);
        }
    }
    
}


public class WalkState : IMovementState
{
    public void HandleInput(HandleManager handleManager)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AnimationStateTableManager.Instance.TryGetAnimationState(out AnimationState newState, AnimationHash.JUMP);
            handleManager.ChangeMovementState(new JumpState());
            return;
        }

        if (handleManager.moveDir.magnitude < 0.3f)
        {
            AnimationStateTableManager.Instance.TryGetAnimationState(out AnimationState newState, AnimationHash.IDLE);
            handleManager.ChangeMovementState(new IdleState());
        }
            
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
        var moveDir = HandleManager.Instance.moveDir;
        if (moveDir.magnitude > 0.3f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            playerRb.rotation = targetRotation;

            playerRb.velocity = moveDir;
        }
    }

    public void TryPlayAnimation(HandleManager mgr)
    {
        if(AnimationUtil.HandleAnimation(mgr.animator, mgr.prevAnimState, AnimationHash.WALK))
        {
            AnimationStateTableManager.Instance.TryGetAnimationState(out AnimationState newState, AnimationHash.WALK);
            mgr.ChangeAnimState(newState);
        }
    }
}

public class IdleState : IMovementState
{
    public void HandleInput(HandleManager handleManager)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AnimationStateTableManager.Instance.TryGetAnimationState(out AnimationState newState, AnimationHash.JUMP);
            handleManager.ChangeMovementState(new JumpState());
            return;
        }
       
        if(handleManager.moveDir.magnitude>0.3f){  
            AnimationStateTableManager.Instance.TryGetAnimationState(out AnimationState newState, AnimationHash.WALK);
            handleManager.ChangeMovementState(new WalkState());
        }
            
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
    }

    public void TryPlayAnimation(HandleManager mgr)
    {
        if (AnimationUtil.IsAnimationEnd(mgr.animator))
        {
            AnimationStateTableManager.Instance.TryGetAnimationState(out AnimationState newState, AnimationHash.IDLE);
            AnimationManager.Instance.Play(mgr.animator, newState.animationHash, 0);
            return;
        }
        
        if(AnimationUtil.HandleAnimation(mgr.animator, mgr.prevAnimState, AnimationHash.IDLE))
        {
            AnimationStateTableManager.Instance.TryGetAnimationState(out AnimationState newState, AnimationHash.IDLE);
            mgr.ChangeAnimState(newState);
        }
    }
}