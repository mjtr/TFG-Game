using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEaterDragonFlyingIdleState : SoulEaterDragonBaseState
{
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("FlyLocomotion");
    private readonly int LocomotionHash = Animator.StringToHash("flyLocomotion");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public SoulEaterDragonFlyingIdleState(SoulEaterDragonStateMachine stateMachine) : base(stateMachine)
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
        stateMachine.AddTimeToLandTime(deltaTime);

        if(stateMachine.GetLandTime() > 7f){
            stateMachine.ResetFlyTime();
            stateMachine.SwitchState(new SoulEaterDragonLandingState(stateMachine));
            return;
        }

        Move(deltaTime);

        if(!IsInChaseRange())
        {
            return;
        }

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            stateMachine.SwitchState(new SoulEaterDragonFlyingChasingState(stateMachine));
            return;
        }
        
        stateMachine.isDetectedPlayed = false;
    }

    public override void Exit(){ }


}
