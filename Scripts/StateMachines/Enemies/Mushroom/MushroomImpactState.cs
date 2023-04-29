using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomImpactState : MushroomBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("MushHit");
    private const float CrossFadeDuration = 0.1f;
    private float duration = 1f;

    public MushroomImpactState(MushroomStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
      
        stateMachine.StopAllCourritines();
        stateMachine.DesactiveAllDemonWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
        
    }
    
    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;
        if(duration <= 0f)
        {   
            stateMachine.SwitchState(new MushroomIdleState(stateMachine));
        }
    }

    public override void Exit(){
        stateMachine.Agent.enabled = true;
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.enabled = false;
        stateMachine.Agent.enabled = true;
    }
}
