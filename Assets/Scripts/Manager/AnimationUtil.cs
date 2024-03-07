using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUtil : MonoBehaviour
{
    public static void HandleAnimation(Animator animator, AnimationState state, int targetHash)
    {
        if (state.animationHash == targetHash)
        {
            if(state.Type == AnimationType.SelfInterruptable)
                AnimationManager.Instance.Play(animator, state.animationHash, 0);
        }
        else
            AnimationManager.Instance.Play(animator, state.animationHash, 0);
    }
}
