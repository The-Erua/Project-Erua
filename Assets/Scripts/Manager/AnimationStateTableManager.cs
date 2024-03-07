using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateTableManager : Singleton<AnimationStateTableManager>
{
    private Dictionary<int, AnimationState> _dictionary = new Dictionary<int, AnimationState>();

    public bool TryGetAnimationState(out AnimationState state, int animHash)
    {
        if (_dictionary.TryGetValue(animHash, out state))
            return true;
        return false;
    }
    public bool TryAddAnimationState(AnimationState state, int animHash)
    {
        if (_dictionary.TryAdd(animHash,state))
            return true;
        return false;
    }
}
