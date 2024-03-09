using System.Collections;
using System.Collections.Generic;
using EditorLog;
using UnityEngine;

public class HandleManager: MonoBehaviourSingleton<HandleManager>
{
    public IMovementState state;
    public Rigidbody playerRb;
    public Animator animator;
    public AnimationState prevAnimState;
    public float impulseVal = 0.78f;
    public Vector3 moveDir;

    public void SetPlayer(Rigidbody playerRb, Animator animator)
    {
        this.playerRb = playerRb;
        this.animator = animator;
        AnimationStateTableManager.Instance.InitAllAnimationState();
        state = new IdleState();
    }

    private void Update()
    {
        if (!playerRb || !animator)
            return;
        state.HandleInput(this);
        state.ExecuteMovement(playerRb);
        state.TryPlayAnimation(this);
    }
    
    public void ChangeMovementState(IMovementState newState) => state = newState;
    public void ChangeAnimState(AnimationState newAnimState) => prevAnimState = newAnimState;

    public void SetMoveDir(Vector3 moveDir)
    {
        this.moveDir = moveDir;
    }
}
