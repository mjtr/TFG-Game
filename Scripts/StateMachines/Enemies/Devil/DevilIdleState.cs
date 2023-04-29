using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilIdleState : DevilBaseState
{

    private const float CrossFadeDuration = 0.1f;
    public DevilIdleState(DevilStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopSounds();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDevilWeapon();
        int IdleHash = getRandomIdleHash();
        stateMachine.Animator.CrossFadeInFixedTime(IdleHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        
        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }

        Move(deltaTime);

        if(!IsInChaseRange() && stateMachine.PatrolPath != null)
        {
            stateMachine.isDetectedPlayed = false;
            stateMachine.SwitchState(new DevilPatrolPathState(stateMachine));
            return;
        }

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            stateMachine.isDetectedPlayed = true;
            if(stateMachine.GetFirsTimeTotSeePlayer())
            {
                stateMachine.SwitchState(new DevilScreamState(stateMachine));
                return;
            }
            stateMachine.SwitchState(new DevilChasingState(stateMachine));
            return;
        }
        
        stateMachine.isDetectedPlayed = false;
    }

    private int getRandomIdleHash()
    {
        int num = Random.Range(0,10);
        if(num <= 5 ){
            return Animator.StringToHash("idle");
        }
       return Animator.StringToHash("idle break");
    }

    public override void Exit(){ }


}
