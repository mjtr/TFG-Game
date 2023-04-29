using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DevilImpactState : DevilBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("hit reaction");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public DevilImpactState(DevilStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.SetFirsTimeToSeePlayer();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDevilWeapon();
        stateMachine.StopSounds();
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.isDetectedPlayed = true;
        if(MustProduceGetHitAnimation())
        {
            stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
        }
        
    }

    private bool MustProduceGetHitAnimation()
    {
        int num = Random.Range(0,20);
        if(num <= 6 ){
            return false;
        }     
        return true;
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        duration -= deltaTime;
        if(duration <= 0f)
        {   
            stateMachine.SwitchState(new DevilIdleState(stateMachine));
        }
    }

    public override void Exit(){ 
        stateMachine.ResetNavhMesh();
    }
}
