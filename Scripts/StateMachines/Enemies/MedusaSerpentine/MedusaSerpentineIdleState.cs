using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaSerpentineIdleState : MedusaSerpentineBaseState
{
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    private float randomIdle;

    public MedusaSerpentineIdleState(MedusaSerpentineStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {   
        GetRandomMedusaIdle();
        stateMachine.SetAudioControllerIsAttacking(false);
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllMedusaSerpentineWeapon();
        stateMachine.Animator.SetFloat(LocomotionHash, randomIdle);
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }

    private void GetRandomMedusaIdle()
    {
        int num = Random.Range(0,10);
        if(num <= 5 ){
            randomIdle = 0f;
            return;
        }
        randomIdle = 0.5f;
    }

    public override void Tick(float deltaTime)
    {
        
        Move(deltaTime);

        if(!IsInChaseRange())
        {
            stateMachine.isDetectedPlayed = false;
            if(stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new MedusaSerpentinePatrolPathState(stateMachine));
                return;
            }
            return;
        }

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            stateMachine.isDetectedPlayed = true;
            stateMachine.ResetNavMesh();
            stateMachine.SwitchState(new MedusaSerpentineChasingState(stateMachine));
            return;
        }
        
        stateMachine.isDetectedPlayed = false;

    }

    public override void Exit(){ }


}
