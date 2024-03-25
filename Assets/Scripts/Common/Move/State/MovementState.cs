using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementState
{
    void HandleInput(HandleManager mgr);
    void ExecuteMovement(Rigidbody playerRb);
    void TryPlayAnimation(HandleManager mgr);
}

public class JumpState : IMovementState
{
    public void HandleInput(HandleManager mgr)
    {
        if (AnimationUtil.IsAnimationEnd(mgr.animator, AnimationHash.JUMP))
        {
            if (AnimationUtil.AirboneWhenOnAir(mgr))
                return;
        }

        if (PlayerMovementManager.Instance.IsCurrentJumpState(CurrentJumpState.Grounded))
        {
            if (AnimationUtil.LandWhenGround(mgr))
                return;
        }
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
        ChrMovementUtil.DoJumpWhenSpaceAndGrounded(playerRb);
    }

    public void TryPlayAnimation(HandleManager mgr)
    {
        AnimationUtil.TryGetAndChangeAnimState(mgr, AnimationHash.JUMP, 0.2f);
    }
}


public class AirboneState : IMovementState
{
    public void HandleInput(HandleManager mgr)
    {
        if (AnimationUtil.LandWhenGround(mgr))
            return;
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
    }

    public void TryPlayAnimation(HandleManager mgr)
    {
        if (AnimationUtil.ReapeatAnimation(mgr, AnimationHash.AIRBONE)) 
            return;
        AnimationUtil.TryGetAndChangeAnimState(mgr, AnimationHash.AIRBONE);
    }
}

public class LandState : IMovementState
{
    public void HandleInput(HandleManager mgr)
    {
        if (AnimationUtil.IsAnimationEnd(mgr.animator, AnimationHash.LAND))
        {
            if (AnimationUtil.IdleWhenGround(mgr))
                return;
        }
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
    }

    public void TryPlayAnimation(HandleManager mgr)
    {
        AnimationUtil.TryGetAndChangeAnimState(mgr, AnimationHash.LAND);
    }
}

public class PrayingState : IMovementState
{
    public void HandleInput(HandleManager mgr)
    {
        if (AnimationUtil.IdleWhenNoMove_NoMouse1(mgr))
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
    public void HandleInput(HandleManager mgr)
    {
        if (AnimationUtil.AirboneWhenOnAir(mgr))
            return;
        if (AnimationUtil.JumpWhenSpaceAndGrounded(mgr)) 
            return;
        // if (AnimationUtil.PrayingWhenMouse1(handleManager))
        //     return;
        if (AnimationUtil.WalkWhenMovingButNoShift(mgr))
            return;
        if (AnimationUtil.IdleWhenNoMove(mgr))
            return;
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
        PlayerMovementManager.Instance.DoRun();
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
    public void HandleInput(HandleManager mgr)
    {
        if (AnimationUtil.AirboneWhenOnAir(mgr))
            return;
        
        if (AnimationUtil.JumpWhenSpaceAndGrounded(mgr)) 
            return;

        if (AnimationUtil.RunWhenMovingAndShift(mgr)) 
            return;

        if (AnimationUtil.IdleWhenNoMove(mgr))
            return;

    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
        PlayerMovementManager.Instance.DoMove();
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
    public void HandleInput(HandleManager mgr)
    {
        if (AnimationUtil.AirboneWhenOnAir(mgr))
            return;
        
        if (AnimationUtil.JumpWhenSpaceAndGrounded(mgr))
            return;
       
        if (AnimationUtil.RunWhenMovingAndShift(mgr)) 
            return;
        
        if (AnimationUtil.WalkWhenMoving(mgr))
            return;
            
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
        // Do nothing
    }

    public void TryPlayAnimation(HandleManager mgr)
    {
        if (AnimationUtil.ReapeatAnimation(mgr, AnimationHash.IDLE)) 
            return;

        AnimationUtil.TryGetAndChangeAnimState(mgr, AnimationHash.IDLE);

    }
}