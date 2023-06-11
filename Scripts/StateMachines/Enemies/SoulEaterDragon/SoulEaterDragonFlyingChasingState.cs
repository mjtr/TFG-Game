using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEaterDragonFlyingChasingState : SoulEaterDragonBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("FlyLocomotion");
    private readonly int LocomotionHash = Animator.StringToHash("flyLocomotion");
    private const float CrossFadeDuration = 0.1f;
    private const float chasingRangeToAdd = 0f;
    private int timeToResetNavMesh = 0;

    public SoulEaterDragonFlyingChasingState(SoulEaterDragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Agent.enabled = true;
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllSoulEaterDragonWeapon();
        
        stateMachine.StartActionMusic();
        stateMachine.SetAudioControllerIsAttacking(true);
        stateMachine.SetChasingRange(stateMachine.PlayerChasingRange + chasingRangeToAdd);
        stateMachine.Animator.SetFloat(LocomotionHash, 1f);
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
        stateMachine.isDetectedPlayed = true;
    }

    public override void Tick(float deltaTime)
    {
        
        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }

        stateMachine.AddTimeToLandTime(deltaTime);

         if(stateMachine.GetLandTime() > 7f){
            stateMachine.ResetLandTime();
            stateMachine.SwitchState(new SoulEaterDragonLandingState(stateMachine));
            return;
        }

        if(isInMagicRange())
        {
            stateMachine.SwitchState(new SoulEaterDragonFlyingFireBreathState(stateMachine));
            return;
        }

        if(!IsInChaseRange())
        {
            stateMachine.SetAudioControllerIsAttacking(false);
            stateMachine.StartAmbientMusic();
            stateMachine.isDetectedPlayed = false;
            stateMachine.SwitchState(new SoulEaterDragonFlyingIdleState(stateMachine));
            return;
        }

        MoveToPlayer(deltaTime);

        FacePlayer();
    }

    private void MoveToPlayer(float deltaTime)
    {   
        if(stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.destination = stateMachine.PlayerHealth.transform.position;

            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed,deltaTime);
        }

        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
        timeToResetNavMesh ++;
        if(timeToResetNavMesh > 300)
        {
            timeToResetNavMesh = 0;
            stateMachine.Agent.ResetPath();
            stateMachine.Agent.enabled = false;
            stateMachine.Agent.enabled = true;
        }
    }

    public override void Exit()
    {
        if(stateMachine.Agent != null)
        {
           stateMachine.ResetNavhMesh();
           stateMachine.Agent.velocity = Vector3.zero;
        }
    }

    private bool isInMagicRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.MaxMagicRange * stateMachine.MaxMagicRange;
    }


}
