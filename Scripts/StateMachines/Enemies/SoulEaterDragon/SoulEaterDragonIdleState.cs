using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEaterDragonIdleState : SoulEaterDragonBaseState
{
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public SoulEaterDragonIdleState(SoulEaterDragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllSoulEaterDragonWeapon();
        stateMachine.Animator.SetFloat(LocomotionHash, 0f);
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        
        Move(deltaTime);

        if(!IsInChaseRange())
        {
            stateMachine.isDetectedPlayed = false;
            if(stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new SoulEaterDragonPatrolPathState(stateMachine));
                return;
            }
            return;
        }

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            stateMachine.SwitchState(new SoulEaterDragonChasingState(stateMachine));
            return;
        }
        
        stateMachine.isDetectedPlayed = false;

        //FacePlayer();
       // stateMachine.Animator.SetFloat(SpeedHash,0f, AnimatorDampTime, deltaTime);
    }

    public override void Exit(){ }


}
