using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMonsterIdleState : CrabMonsterBaseState
{

    private const float CrossFadeDuration = 0.1f;
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");

    public CrabMonsterIdleState(CrabMonsterStateMachine stateMachine) : base(stateMachine)
    {}

    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllCrabMonsterWeapon();
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
                stateMachine.SwitchState(new CrabMonsterPatrolPathState(stateMachine));
                return;
            }
            return;
        }

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            stateMachine.isDetectedPlayed = true;
            if(stateMachine.GetFirsTimeTotSeePlayer())
            {
                stateMachine.SwitchState(new CrabMonsterScreamState(stateMachine));
                return;
            }
            stateMachine.SwitchState(new CrabMonsterChasingState(stateMachine));
            return;
        }
        
        stateMachine.isDetectedPlayed = false;
    }

    public override void Exit(){ }


}
