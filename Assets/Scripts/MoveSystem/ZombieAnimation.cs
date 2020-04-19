using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimation : LowerAnimator
{

    public Animator zombieAnimator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void SetAnimationState(in Vector2 velocity)
    {
        zombieAnimator.SetFloat("RunSpeed", velocity.magnitude);
        
    }
}
