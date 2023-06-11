using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityDemonIdleState : InfinityDemonBaseState
{
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public InfinityDemonIdleState(InfinityDemonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.SetAudioControllerIsAttacking(false);
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllInfinityDemonWeapon();
        stateMachine.Animator.SetFloat(LocomotionHash, 0f);
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        
        Move(deltaTime);

        if(!IsInChaseRange())
        {
            //Vamos a hacer aquí que el dragon patrulle
            stateMachine.isDetectedPlayed = false;
            if(stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new InfinityDemonPatrolPathState(stateMachine));
                return;
            }
            return;
        }

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            stateMachine.isDetectedPlayed = true;
            stateMachine.ResetNavMesh();
            stateMachine.SwitchState(new InfinityDemonChasingState(stateMachine));
            return;
        }
        
        stateMachine.isDetectedPlayed = false;
    }

    public override void Exit(){ }


}
