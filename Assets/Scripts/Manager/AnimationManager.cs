using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class AnimationManager: Singleton<AnimationManager>
{

    public void SetBool(Animator animator, int id, bool value) => animator.SetBool(id, value);
    public void SetInt(Animator animator, int id, int value) => animator.SetInteger(id, value);
    public void SetFloat(Animator animator, int id, float value) => animator.SetFloat(id, value);
    public void SetTrigger(Animator animator, int id) => animator.SetTrigger(id);
    
    public void Stop(Animator animator) => animator.enabled = false;

    public void Play(Animator animator, int stateHash, int layer)
    {
        animator.enabled = true;
        animator.Play(stateHash,layer,0f);
    }
        
}