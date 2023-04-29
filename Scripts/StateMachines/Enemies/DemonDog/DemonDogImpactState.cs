using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonDogImpactState : DemonDogBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("GetHit1");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public DemonDogImpactState(DemonDogStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.DesactiveAllDemonDogWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;
        if(duration <= 0f)
        {
            stateMachine.SwitchState(new DemonDogIdleState(stateMachine));
        }
    }

    public override void Exit(){
        stateMachine.ResetNavMesh();
    }
}
