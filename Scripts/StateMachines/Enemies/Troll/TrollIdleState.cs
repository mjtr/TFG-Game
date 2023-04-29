using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollIdleState : TrollBaseState
{

    private const float CrossFadeDuration = 0.1f;
    public TrollIdleState(TrollStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ResetNavhMesh();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllTrollWeapon();
        int IdleHash = getRandomIdleHash();
        stateMachine.Animator.CrossFadeInFixedTime(IdleHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        
        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }

        Move(deltaTime);

        if(!IsInChaseRange())
        {
            stateMachine.isDetectedPlayed = false;
            if(stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new TrollPatrolPathState(stateMachine));
                return;
            }
            return;
        }

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            stateMachine.isDetectedPlayed = true;
            if(stateMachine.GetFirsTimeTotSeePlayer())
            {
                stateMachine.SwitchState(new TrollScreamState(stateMachine));
                return;
            }
            stateMachine.SwitchState(new TrollChasingState(stateMachine));
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
       return Animator.StringToHash("Idle_other");
    }

    public override void Exit(){
        stateMachine.ResetNavhMesh();
    }


}
