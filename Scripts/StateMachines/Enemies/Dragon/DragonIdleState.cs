using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonIdleState : DragonBaseState
{

    private readonly int IdleHash = Animator.StringToHash("IdleBreak");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public DragonIdleState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.StopDragonSounds();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDragonWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(IdleHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        
        Move(deltaTime);

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            stateMachine.SwitchState(new DragonChasingState(stateMachine));
            return;
        }
        else{
            stateMachine.isDetectedPlayed = false;
        }

    }

    public override void Exit(){ }


}
