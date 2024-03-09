using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    None =-1,
    SelfInterruptable = 0,
    NonSelfInterruptable = 1,
}
public struct AnimationState
{

    public AnimationType Type { get; set; }
    public int animationHash;

    public AnimationState(AnimationType type, int animationHash)
    {
        Type = type;
        this.animationHash = animationHash;
    }
}
