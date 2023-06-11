using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniDinoImpactState : MiniDinoBaseState
{

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public MiniDinoImpactState(MiniDinoStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        int impactHash = getRandomDinoGetHitAnimation();
        stateMachine.DesactiveAllMiniDinoWeapon();
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.isDetectedPlayed = true;
        if(MustProduceGetHitAnimation())
        {
            stateMachine.Animator.CrossFadeInFixedTime(impactHash, CrossFadeDuration);
        }
    }

    private int getRandomDinoGetHitAnimation()
    {
        int num = Random.Range(0,10);
        if(num <= 5 ){
            return Animator.StringToHash("GotHit");
        }
       return Animator.StringToHash("GotHit_body");
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
            stateMachine.SwitchState(new MiniDinoIdleState(stateMachine));
        }
    }

    public override void Exit(){
        stateMachine.Agent.enabled = true;
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.enabled = false;
        stateMachine.Agent.enabled = true;
    }
}
