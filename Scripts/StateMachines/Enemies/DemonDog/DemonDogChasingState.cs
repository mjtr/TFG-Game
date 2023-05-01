using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonDogChasingState : DemonDogBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");

    private readonly int SpeedHash = Animator.StringToHash("Speed");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private int timeToResetNavMesh = 0;

    private bool isFirstTime = true;

    public DemonDogChasingState(DemonDogStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        if(isFirstTime)
        {
            isFirstTime = false;
            stateMachine.SetPlayerChasingRange(stateMachine.PlayerChasingRange + 4f);
        }
        stateMachine.StartActionMusic();
        stateMachine.SetAudioControllerIsAttacking(true);
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDemonDogWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {

        if(!IsInChaseRange())
        {

            stateMachine.SetAudioControllerIsAttacking(false);
            stateMachine.StartAmbientMusic();
            stateMachine.isDetectedPlayed = false;
            if(stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new DemongDogPatrolPathState(stateMachine));
                return; 
            }
            stateMachine.SwitchState(new DemonDogIdleState(stateMachine));
            return; 
        
        }else if(isInAttackRange()){
            stateMachine.isDetectedPlayed = true;
            stateMachine.SwitchState(new DemonDogAttackingState(stateMachine));
            return;
        }

        MoveToPlayer(deltaTime);

        FacePlayer();

        stateMachine.Animator.SetFloat(SpeedHash,1f, AnimatorDampTime, deltaTime);
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
            stateMachine.ResetNavMesh();
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

}
