using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TitanImpactState : TitanBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("GetHit");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public TitanImpactState(TitanStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.SetFirsTimeToSeePlayer();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllTitanWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        duration -= deltaTime;
        if(duration <= 0f)
        {   
            stateMachine.SwitchState(new TitanIdleState(stateMachine));
        }
    }

    public override void Exit(){ 
        stateMachine.ResetNavhMesh();
    }
}
