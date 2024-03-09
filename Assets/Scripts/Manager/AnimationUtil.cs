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
            AnimationManager.Instance.Play(animator, newState.animationHash, 0);
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
}
