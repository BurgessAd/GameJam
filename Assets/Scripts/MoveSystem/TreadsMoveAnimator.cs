using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreadsMoveAnimator : LowerAnimator
{
    private AimComponent treadsAim;
    private Animator treadsAnimator;
    private void Awake()
    {
        treadsAnimator = GetComponent<Animator>();
        treadsAim = GetComponent<AimComponent>();
    }
    public override void SetAnimationState(in Vector2 velocity)
    {
        treadsAnimator.SetFloat("RunSpeed", velocity.magnitude);
        treadsAim.SetDesiredLookDirection(velocity);
    }
}
