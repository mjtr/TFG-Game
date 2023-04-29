using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonDogIdleState : DemonDogBaseState
{

     private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");

    private readonly int SpeedHash = Animator.StringToHash("Speed");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;


    public DemonDogIdleState(DemonDogStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDemonDogWeapon();
        stateMachine.Animator.SetFloat(SpeedHash, 0f);
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
                stateMachine.SwitchState(new DemongDogPatrolPathState(stateMachine));
                return;
            }
            return;
        }

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            stateMachine.isDetectedPlayed = true;
            stateMachine.SwitchState(new DemonDogChasingState(stateMachine));
            return;
        }
        
        stateMachine.isDetectedPlayed = false;        
    }

    public override void Exit(){ }


}
