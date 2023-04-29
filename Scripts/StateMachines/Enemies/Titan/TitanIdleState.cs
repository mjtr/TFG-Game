using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanIdleState : TitanBaseState
{

    private const float CrossFadeDuration = 0.1f;
    public TitanIdleState(TitanStateMachine stateMachine) : base(stateMachine)
    {}

    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllTitanWeapon();
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
                stateMachine.SwitchState(new TitanPatrolPathState(stateMachine));
                return;
            }
            return;
        }

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            stateMachine.isDetectedPlayed = true;
            if(stateMachine.GetFirsTimeTotSeePlayer())
            {
                stateMachine.SwitchState(new TitanScreamState(stateMachine));
                return;
            }
            stateMachine.SwitchState(new TitanChasingState(stateMachine));
            return;
        }
        
        stateMachine.isDetectedPlayed = false;
    }

    private int getRandomIdleHash()
    {
        return Animator.StringToHash("Idle");
    }

    public override void Exit(){ }


}
