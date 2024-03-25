using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChrMovementUtil : MonoBehaviour
{
    public static void DoJumpWhenSpaceAndGrounded(Rigidbody playerRb)
    {
        if (Input.GetKeyDown(KeyCode.Space) &&PlayerMovementManager.Instance.IsCurrentJumpState(CurrentJumpState.Grounded))
        {
            PlayerMovementManager.Instance.CurrentJumpState = CurrentJumpState.ReadyToJump;
        }
    }

}
