using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockMonsterImpactState : RockMonsterBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("GotHit");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public RockMonsterImpactState(RockMonsterStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllRockMonsterWeapon();
        stateMachine.StopSounds();
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        duration -= deltaTime;
        if(duration <= 0f)
        {   
            stateMachine.SwitchState(new RockMonsterIdleState(stateMachine));
        }
    }

    public override void Exit(){ 
        stateMachine.ResetNavMesh();
    }
}
