using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelociraptorIdleState : VelociraptorBaseState
{
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");
    private const float CrossFadeDuration = 0.1f;
    public VelociraptorIdleState(VelociraptorStateMachine stateMachine) : base(stateMachine)
    {}

    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllVelociraptorWeapon();
        int IdleHash = getRandomIdleHash();
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
        stateMachine.Animator.SetFloat(LocomotionHash,0f);
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
                stateMachine.SwitchState(new VelociraptorPatrolPathState(stateMachine));
                return;
            }
            return;
        }

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            stateMachine.isDetectedPlayed = true;
            if(stateMachine.GetFirsTimeTotSeePlayer())
            {
                stateMachine.SwitchState(new VelociraptorScreamState(stateMachine));
                return;
            }
            stateMachine.SwitchState(new VelociraptorChasingState(stateMachine));
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
