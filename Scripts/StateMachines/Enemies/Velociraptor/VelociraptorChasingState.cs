using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VelociraptorChasingState : VelociraptorBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");
    private const float CrossFadeDuration = 0.1f;
    private int timeToResetNavMesh = 0;
    private bool firsTimeToFollowCharater = true;

    public VelociraptorChasingState(VelociraptorStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {   
        stateMachine.Agent.enabled = true;
        stateMachine.SetFirsTimeToSeePlayer();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllVelociraptorWeapon();
        
        if(firsTimeToFollowCharater)
        {
            firsTimeToFollowCharater = false;
            stateMachine.SetChasingRange(stateMachine.PlayerChasingRange + 3f);
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

        if(stateMachine.GetIsFirstTimeToCallAllies() && !stateMachine.GetHasHalfLife())
        {
            stateMachine.SetHasHalfLife(stateMachine.Health.GetHealthPoints() < (stateMachine.Health.GetMaxHealthPoints() / 2));
        }

        if(stateMachine.GetIsFirstTimeToCallAllies() && stateMachine.GetHasHalfLife())
        {
            stateMachine.SetIsFirstTimeToCallAllies(false);
            stateMachine.SwitchState(new VelociraptorCallingState(stateMachine));
            return;
        }

        if(!IsInChaseRange())
        {
          
            stateMachine.isDetectedPlayed = false;
            stateMachine.SetAudioControllerIsAttacking(false);
            stateMachine.StartAmbientMusic();
            if(stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new VelociraptorPatrolPathState(stateMachine));
                return;
            }
            return;
        
        }else if(isInAttackRange()){
            stateMachine.isDetectedPlayed = true;
            stateMachine.SwitchState(new VelociraptorAttackingState(stateMachine));
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
            stateMachine.ResetNavhMesh();
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
