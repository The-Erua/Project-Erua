using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using EditorLog;
using UnityEngine;

public class AnimationStateTableManager : Singleton<AnimationStateTableManager>
{
    private Dictionary<int, AnimationState> _dictionary = new Dictionary<int, AnimationState>();

    const string FilePath = "Assets/TextAssets/AnimationStateDefinition.txt";

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

    public void InitAllAnimationState()
    {
        try
        {
            string[] lines = File.ReadAllLines(FilePath);
            foreach (var line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2)
                {
                    string animName = parts[0].Trim();
                    AnimationType animType = (AnimationType)Enum.Parse(typeof(AnimationType), parts[1].Trim());

                    int hash = Animator.StringToHash(animName);
                    AnimationUtil.InitAnimState(hash, animType);
                }
            }
        }
        catch (Exception e)
        {
            EditorDebug.LogError("Error reading AnimationStateDefinition.txt: " + e.Message);
            throw;
        }
    }
}
