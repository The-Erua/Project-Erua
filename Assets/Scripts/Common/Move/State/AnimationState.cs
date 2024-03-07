using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    SelfInterruptable = 0,
    NonSelfInterruptable = 1,
}
public class AnimationState
{

    public AnimationType Type { get; set; }
    public int animationHash;

    public AnimationState(AnimationType type, int animationHash)
    {
        Type = type;
        this.animationHash = animationHash;
    }
}
