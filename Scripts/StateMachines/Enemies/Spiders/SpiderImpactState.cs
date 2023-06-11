using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderImpactState : SpiderBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("Got Hit");
    private const float CrossFadeDuration = 0.1f;
    private float duration = 1f;

    public SpiderImpactState(SpiderStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.StopParticlesEffects();
        stateMachine.StopAllCourritines();
        stateMachine.DesactiveAllSpiderWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }
    
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        stateMachine.AddTimeToScreamTime(deltaTime);    

        duration -= deltaTime;
        if(duration <= 0f)
        {   
            stateMachine.SwitchState(new SpiderIdleState(stateMachine));
        }
    }

    public override void Exit(){
        stateMachine.Agent.enabled = true;
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.enabled = false;
        stateMachine.Agent.enabled = true;
    }
}
