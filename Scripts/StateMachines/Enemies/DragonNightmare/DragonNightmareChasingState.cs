using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DragonNightmareChasingState : DragonNightmareBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private int timeToResetNavMesh = 0;
    private bool firsTimeToFollowCharater = true;
    public DragonNightmareChasingState(DragonNightmareStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDragonNightmareWeapon();
        if(firsTimeToFollowCharater)
        {
            firsTimeToFollowCharater = false;
            stateMachine.SetChasingRange(stateMachine.PlayerChasingRange + 5f);            
        }

        stateMachine.StartActionMusic();
        stateMachine.SetAudioControllerIsAttacking(true);
        stateMachine.Animator.SetFloat(LocomotionHash, 1f);
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
        stateMachine.isDetectedPlayed = true;
    }

    public override void Tick(float deltaTime)
    {
        
        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }

        if(!IsInChaseRange())
        {
            stateMachine.SetAudioControllerIsAttacking(false);
            stateMachine.StartAmbientMusic();
            stateMachine.isDetectedPlayed = false;
            stateMachine.SwitchState(new DragonNightmarePatrolPathState(stateMachine));
            return;
        
        }
        else if(isInBasicAttackRange()){
            stateMachine.isDetectedPlayed = true;
            stateMachine.SwitchState(new DragonNightmareBasicAttackState(stateMachine));
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

    private bool isInBasicAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }

}
