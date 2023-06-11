using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEaterDragonImpactState : SoulEaterDragonBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("Get Hit");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public SoulEaterDragonImpactState(SoulEaterDragonStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.ResetNavhMesh();
        stateMachine.StopAllCourritines();
        stateMachine.DesactiveAllSoulEaterDragonWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        duration -= deltaTime;
        if(duration <= 0f)
        {   
            stateMachine.SwitchState(new SoulEaterDragonIdleState(stateMachine));
        }
    }

    public override void Exit(){
        
     }
}
