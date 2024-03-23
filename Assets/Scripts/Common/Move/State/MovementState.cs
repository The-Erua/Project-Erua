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
        AnimationUtil.IdleWhenGround(handleManager);
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
        AnimationUtil.DoJumpWhenSpaceAndGrounded(playerRb);
    }

    public void TryPlayAnimation(HandleManager mgr)
    {
        AnimationUtil.TryGetAndChangeAnimState(mgr, AnimationHash.JUMP);
    }
}
public class PrayingState : IMovementState
{
    public void HandleInput(HandleManager handleManager)
    {
        if (AnimationUtil.IdleWhenNoMove_NoMouse1(handleManager))
            return;
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
        //DoNothing
    }

    public void TryPlayAnimation(HandleManager mgr)
    {
        if (AnimationUtil.ReapeatAnimation(mgr, AnimationHash.PRAYING)) 
            return;

        AnimationUtil.TryGetAndChangeAnimState(mgr, AnimationHash.PRAYING);
    }
}

public class RunState : IMovementState
{
    public void HandleInput(HandleManager handleManager)
    {
        if (AnimationUtil.JumpWhenSpaceAndGrounded(handleManager)) 
            return;
        // if (AnimationUtil.PrayingWhenMouse1(handleManager))
        //     return;
        if (AnimationUtil.WalkWhenMovingButNoShift(handleManager))
            return;
        if (AnimationUtil.IdleWhenNoMove(handleManager))
            return;
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
        var moveDir = HandleManager.Instance.moveDir * HandleManager.Instance.speedMultiplier;
        AnimationUtil.DoMove(playerRb, moveDir);
    }

    public void TryPlayAnimation(HandleManager mgr)
    {
        if (AnimationUtil.ReapeatAnimation(mgr, AnimationHash.RUN)) 
            return;

        AnimationUtil.TryGetAndChangeAnimState(mgr, AnimationHash.RUN);
    }
}


public class WalkState : IMovementState
{
    public void HandleInput(HandleManager handleManager)
    {
        if (AnimationUtil.JumpWhenSpaceAndGrounded(handleManager)) 
            return;

        if (AnimationUtil.RunWhenMovingAndShift(handleManager)) 
            return;

        if (AnimationUtil.IdleWhenNoMove(handleManager))
            return;

    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
        var moveDir = HandleManager.Instance.moveDir;
        AnimationUtil.DoMove(playerRb, moveDir);
    }


    public void TryPlayAnimation(HandleManager mgr)
    {
        if (AnimationUtil.ReapeatAnimation(mgr, AnimationHash.WALK)) 
            return;

        AnimationUtil.TryGetAndChangeAnimState(mgr, AnimationHash.WALK);

    }
}

public class IdleState : IMovementState
{
    public void HandleInput(HandleManager handleManager)
    {
        if (AnimationUtil.JumpWhenSpaceAndGrounded(handleManager))
            return;
       
        if (AnimationUtil.RunWhenMovingAndShift(handleManager)) 
            return;
        
        if(AnimationUtil.WalkWhenMoving(handleManager))
            return;
            
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
    }

    public void TryPlayAnimation(HandleManager mgr)
    {
        if (AnimationUtil.ReapeatAnimation(mgr, AnimationHash.IDLE)) return;

        AnimationUtil.TryGetAndChangeAnimState(mgr, AnimationHash.IDLE);

    }
}