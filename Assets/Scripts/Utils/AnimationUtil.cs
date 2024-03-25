using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUtil : MonoBehaviour
{
    private static bool IsGrounded => PlayerMovementManager.Instance.IsCurrentJumpState(CurrentJumpState.Grounded);
    public static bool HandleAnimation(Animator animator, AnimationState currentState, int targetHash, float crossfadeTime = 0.05f)
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
            AnimationManager.Instance.CrossFade(animator, 0.05f, newState.animationHash, 0);
            return true;
        }
    }
    
    public static void InitAnimState(int hash, AnimationType type)
    {
        AnimationStateTableManager.Instance.TryAddAnimationState(new AnimationState(type, hash), hash);
    }
    
    public static bool IsAnimationEnd(Animator animator, int hashId)
    {
        int layerIndex = 0;
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
        return stateInfo.normalizedTime >= 1 && stateInfo.shortNameHash == hashId;
    }
    
    public static bool ReapeatAnimation(HandleManager mgr, int hash)
    {
        if (IsAnimationEnd(mgr.animator, hash))
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


    public static bool IdleWhenGround(HandleManager handleManager)
    {
        if (IsGrounded)
        {
            handleManager.ChangeMovementState(new IdleState());
            return true;
        }

        return false;
    }
    
    public static bool RunWhenMovingAndShift(HandleManager handleManager)
    {
        if (Input.GetKey(KeyCode.LeftShift) && handleManager.moveDir.magnitude > 0.3f)
        {
            handleManager.ChangeMovementState(new RunState());
            return true;
        }

        return false;
    }
    
    public static bool JumpWhenSpaceAndGrounded(HandleManager handleManager)
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
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
    
    public static void TryGetAndChangeAnimState(HandleManager mgr, int hash, float crossFadeTime = 0.05f)
    {
        if(AnimationUtil.HandleAnimation(mgr.animator, mgr.prevAnimState, hash, crossFadeTime))
        {
            AnimationStateTableManager.Instance.TryGetAnimationState(out AnimationState newState, hash);
            mgr.ChangeAnimState(newState);
        }
    }
    
    public static bool PrayingWhenMouse1(HandleManager handleManager)
    {
        if (PrayingAction())
            return true;
        return false;
        
        bool PrayingAction()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && IsGrounded)
            {
                handleManager.ChangeMovementState(new PrayingState());
                return true;
            }

            return false;
        }
    }

    public static bool AirboneWhenOnAir(HandleManager handleManager)
    {
        if (PlayerMovementManager.Instance.IsCurrentJumpState(CurrentJumpState.OnAir))
        {
            handleManager.ChangeMovementState(new AirboneState());
            return true;
        }

        return false;
    }

    
    public static bool LandWhenGround(HandleManager handleManager)
    {
        if (IsGrounded)
        {
            handleManager.ChangeMovementState(new LandState());
            return true;
        }

        return false;
    }
}
