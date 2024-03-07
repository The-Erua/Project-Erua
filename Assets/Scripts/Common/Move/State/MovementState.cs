using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovementState
{
    void HandleInput(HandleManager handleManager);
    void ExecuteMovement(Rigidbody playerRb);
    void TryPlayAnimation(Animator animator, AnimationState state);
}
public class JumpState : IMovementState
{
    public void HandleInput(HandleManager handleManager)
    {
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
    }

    public void TryPlayAnimation(Animator animator, AnimationState state)
    {
        AnimationUtil.HandleAnimation(animator, state, CharacterMovement.AnimationIntHash.JUMP);
    }
}

public class DashState : IMovementState
{
    public void HandleInput(HandleManager handleManager)
    {
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
    }

    public void TryPlayAnimation(Animator animator, AnimationState state)
    {
        AnimationUtil.HandleAnimation(animator, state, CharacterMovement.AnimationIntHash.DASH);
    }
}


public class WalkState : IMovementState
{
    public void HandleInput(HandleManager handleManager)
    {
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
    }

    public void TryPlayAnimation(Animator animator, AnimationState state)
    {
        AnimationUtil.HandleAnimation(animator, state, CharacterMovement.AnimationIntHash.WALK);
    }
}

public class IdleState : IMovementState
{
    public void HandleInput(HandleManager handleManager)
    {
    }

    public void ExecuteMovement(Rigidbody playerRb)
    {
        
    }

    public void TryPlayAnimation(Animator animator, AnimationState state)
    {
        AnimationUtil.HandleAnimation(animator, state, CharacterMovement.AnimationIntHash.IDLE);
    }
}

public class HandleManager: MonoBehaviourSingleton<HandleManager>
{
    public IMovementState state;
    public Rigidbody playerRb;
    public Animator animator;
    private AnimationState animState;
    protected override void Awake()
    {
        base.Awake();
        state = new IdleState();
        animState = new AnimationState(AnimationType.NonSelfInterruptable, CharacterMovement.AnimationIntHash.IDLE);
    }

    private void Update()
    {
        state.HandleInput(this);
        state.ExecuteMovement(playerRb);
        state.TryPlayAnimation(animator, animState);
    }
    
    public void ChangeState(IMovementState newState, AnimationState newAnimState)
    {
        state = newState;
        animState = newAnimState;
    }
}