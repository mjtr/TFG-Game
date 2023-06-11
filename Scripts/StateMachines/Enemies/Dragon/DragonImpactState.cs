using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonImpactState : DragonBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("Hit01");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public DragonImpactState(DragonStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.DesactiveAllDragonWeapon();
        stateMachine.StopAllCourritines();
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        duration -= deltaTime;
        if(duration <= 0f)
        {   
            stateMachine.SwitchState(new DragonIdleState(stateMachine));
        }
    }

    public override void Exit(){ 
        stateMachine.ResetNavhMesh();
    }
}
