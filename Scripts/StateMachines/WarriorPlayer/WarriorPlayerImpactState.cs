using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorPlayerImpactState : WarriorPlayerBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("Warrior_Get_Hit");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public WarriorPlayerImpactState(WarriorPlayerStateMachine stateMachine) : base(stateMachine){  }

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
