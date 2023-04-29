using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniDinoIdleState : MiniDinoBaseState
{
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public MiniDinoIdleState(MiniDinoStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {

        int IdleHash = getRandomIdleHash();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllMiniDinoWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(IdleHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        
        Move(deltaTime);

        if(!IsInChaseRange())
        {
            stateMachine.isDetectedPlayed = false;
            stateMachine.SwitchState(new MiniDinoPatrolPathState(stateMachine));
            return;
        }

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            stateMachine.isDetectedPlayed = true;
            stateMachine.SwitchState(new MiniDinoChasingState(stateMachine));
            return;
        }
        
        stateMachine.isDetectedPlayed = false;
    }

    private int getRandomIdleHash()
    {
        int num = Random.Range(0,10);
        if(num <= 5 ){
            return Animator.StringToHash("Idle");
        }
       return Animator.StringToHash("Idle_break");
    }

    public override void Exit(){ }


}
