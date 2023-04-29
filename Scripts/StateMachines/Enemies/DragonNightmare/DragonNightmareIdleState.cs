using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonNightmareIdleState : DragonNightmareBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    public DragonNightmareIdleState(DragonNightmareStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDragonNightmareWeapon();
        int IdleHash = getRandomIdleHash();
        stateMachine.Animator.SetFloat(LocomotionHash, 0f);
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        
        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }

        Move(deltaTime);

        if(!IsInChaseRange() && stateMachine.PatrolPath != null)
        {
            stateMachine.isDetectedPlayed = false;
            stateMachine.SwitchState(new DragonNightmarePatrolPathState(stateMachine));
            return;
        }

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            if(stateMachine.GetFirstTimeToSeePlayer()){
                stateMachine.SetFirstTimeToSeePlayer(false);
                stateMachine.isDetectedPlayed = true;
                stateMachine.SwitchState(new DragonNightmareScreamState(stateMachine));
                return;
            }

            stateMachine.isDetectedPlayed = true;
            stateMachine.SwitchState(new DragonNightmareChasingState(stateMachine));
            return;
        }
        
        stateMachine.isDetectedPlayed = false;
    }

    private int getRandomIdleHash()
    {
        return Animator.StringToHash("Idle01");
    }

    public override void Exit(){ }


}
