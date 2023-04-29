using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerImpactState : HumanPlayerBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("GotHit");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public HumanPlayerImpactState(HumanPlayerStateMachine stateMachine) : base(stateMachine){  }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }



    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;

        if(duration <= 0f)
        {
            ReturnToLocomotion();
        }

    }

    public override void Exit(){  }

}
