using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUtil : MonoBehaviour
{
    public static bool HandleAnimation(Animator animator, AnimationState currentState, int targetHash)
    {
        AnimationStateTableManager.Instance.TryGetAnimationState(out AnimationState newState, targetHash);
        if (currentState.Type != AnimationType.None && currentState.animationHash == targetHash)
        {
            if (currentState.Type == AnimationType.SelfInterruptable)
            {
                AnimationManager.Instance.Play(animator, newState.animationHash, 0);
                return true;
            }
            else
                return false;
        }
        else
        {
            AnimationManager.Instance.CrossFade(animator, 0.1f, newState.animationHash, 0);
            return true;
        }
    }
    
    public static void InitAnimState(int hash, AnimationType type)
    {
        AnimationStateTableManager.Instance.TryAddAnimationState(new AnimationState(type, hash), hash);
    }
    
    public static bool IsAnimationEnd(Animator animator)
    {
        int layerIndex = 0;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
        return stateInfo.normalizedTime >= 1;
    }
    
    public static bool ReapeatAnimation(HandleManager mgr, int hash)
    {
        if (IsAnimationEnd(mgr.animator))
        {
            AnimationStateTableManager.Instance.TryGetAnimationState(out AnimationState newState, hash);
            AnimationManager.Instance.Play(mgr.animator, newState.animationHash, 0);
            return true;
        }

        return false;
    }
    
    public static bool IdleWhenNoMove(HandleManager handleManager)
    {
        if (handleManager.moveDir.magnitude < 0.1f)
        {
            handleManager.ChangeMovementState(new IdleState());
            return true;
        }

        return false;
    }
    
    public static bool IdleWhenNoMove_NoMouse1(HandleManager handleManager)
    {
        if (handleManager.moveDir.magnitude < 0.1f && !Input.GetKey(KeyCode.Mouse1))
        {
            handleManager.ChangeMovementState(new IdleState());
            return true;
        }

        return false;
    }

    
    public static void IdleWhenGround(HandleManager handleManager)
    {
        if (handleManager.isGrounded)
        {
            PlayerVelocityManager.Instance.SetJump(false);
            handleManager.ChangeMovementState(new IdleState());
        }
    }
    
    public static void DoMove(Rigidbody playerRb, Vector3 moveDir)
    {
        if (moveDir.magnitude > 0.3f)
        {
            PlayerVelocityManager.Instance.DoMove();
        }
    }
    
    public static bool RunWhenMovingAndShift(HandleManager handleManager)
    {
        if (Input.GetKey(KeyCode.LeftShift) && handleManager.moveDir.magnitude > 0.1f)
        {
            handleManager.ChangeMovementState(new RunState());
            return true;
        }

        return false;
    }
    
    public static bool JumpWhenSpaceAndGrounded(HandleManager handleManager)
    {
        if (Input.GetKeyDown(KeyCode.Space) && handleManager.isGrounded)
        {
            handleManager.ChangeMovementState(new JumpState());
            return true;
        }

        return false;
    }

    public static bool WalkWhenMoving(HandleManager handleManager)
    {
        if(handleManager.moveDir.magnitude > 0.1f){  
            handleManager.ChangeMovementState(new WalkState());
            return true;
        }

        return false;
    }
    
    public static bool WalkWhenMovingButNoShift(HandleManager handleManager)
    {
        var key = Input.GetKey(KeyCode.LeftShift);
        if (!key && handleManager.moveDir.magnitude > 0.1f)
        {
            handleManager.ChangeMovementState(new WalkState());
            return true;
        }

        return false;
    }
    
    public static void TryGetAndChangeAnimState(HandleManager mgr, int hash)
    {
        if(AnimationUtil.HandleAnimation(mgr.animator, mgr.prevAnimState, hash))
        {
            AnimationStateTableManager.Instance.TryGetAnimationState(out AnimationState newState, hash);
            mgr.ChangeAnimState(newState);
        }
    }

    public static void DoJumpWhenSpaceAndGrounded(Rigidbody playerRb)
    {
        if (Input.GetKeyDown(KeyCode.Space) && HandleManager.Instance.isGrounded)
        {
            PlayerVelocityManager.Instance.SetJump(true);
            HandleManager.Instance.isGrounded = false;
        }
    }

    public static bool PrayingWhenMouse1(HandleManager handleManager)
    {
        if (PrayingAction())
            return true;
        return false;
        
        bool PrayingAction()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && HandleManager.Instance.isGrounded)
            {
                handleManager.ChangeMovementState(new PrayingState());
                return true;
            }

            return false;
        }
    }
}
