using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonChasingState : DragonBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Ground Locomotion");

    private readonly int LocomotionHash = Animator.StringToHash("locomotion");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    private const float chasingRangeToAdd = 2.1f;

    private int timeToResetNavMesh = 0;

    public DragonChasingState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.isDetectedPlayed = true;
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDragonWeapon();
        stateMachine.SetAudioControllerIsAttacking(true);
        stateMachine.StartActionMusic();
        stateMachine.SetChasingRange(stateMachine.PlayerChasingRange + chasingRangeToAdd);
        stateMachine.Animator.SetFloat(LocomotionHash, 1f);
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        
        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }
        
        if(!IsInChaseRange())
        {
            stateMachine.SetAudioControllerIsAttacking(false);
            stateMachine.StartAmbientMusic();
            stateMachine.isDetectedPlayed = false;
            stateMachine.SwitchState(new DragonIdleState(stateMachine));
            return;
        
        }else if(isInAttackRange()){
            stateMachine.SwitchState(new DragonAttackingState(stateMachine));
            return;
        }else if(isInFireBreathAttackRange() ){
            stateMachine.SwitchState(new DragonFireBreathState(stateMachine));
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
        if(timeToResetNavMesh > 200)
        {
            timeToResetNavMesh = 0;
            stateMachine.Agent.enabled = true;
            stateMachine.Agent.ResetPath();
            stateMachine.Agent.enabled = false;
            stateMachine.Agent.enabled = true;
        }
    }

    public override void Exit()
    {
        if(stateMachine.Agent != null && stateMachine.Agent.enabled)
        {
           stateMachine.Agent.ResetPath();
           stateMachine.Agent.velocity = Vector3.zero;
        }

    }
    private bool isInAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }

    private bool isInFireBreathAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.MaxFireBreathAttackRange * stateMachine.MaxFireBreathAttackRange 
            && playerDistanceSqr >= stateMachine.MinFireBreathAttackRange * stateMachine.MinFireBreathAttackRange ;
    }

}
