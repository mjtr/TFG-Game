using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrabMonsterChasingState : CrabMonsterBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");
    private const float CrossFadeDuration = 0.1f;
    private int timeToResetNavMesh = 0;
    private bool firsTimeToFollowCharater = true;

    public CrabMonsterChasingState(CrabMonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {   
        stateMachine.Agent.enabled = true;
        stateMachine.SetFirsTimeToSeePlayer();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllCrabMonsterWeapon();
        
        if(firsTimeToFollowCharater)
        {
            firsTimeToFollowCharater = false;
            stateMachine.SetChasingRange(stateMachine.PlayerChasingRange + 160f);
        }

        if(stateMachine.GetFirstTimeToPlayEpicMusic())
        {
            stateMachine.StartEpicMusic();
            stateMachine.SetFirstTimeToPlayEpicMusic();
        }
        
        
        stateMachine.Animator.SetFloat(LocomotionHash, 1f);
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
        stateMachine.isDetectedPlayed = true;
    }

    public override void Tick(float deltaTime)
    {
        
        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }

        if(!IsInChaseRange())
        {
            
            stateMachine.isDetectedPlayed = false;
            if(stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new CrabMonsterPatrolPathState(stateMachine));
                return;
            }
            return;
        
        }else {
            stateMachine.AddTimeToHealTime(deltaTime);
            stateMachine.AddTimeToExplosionTime(deltaTime);

            if(stateMachine.GetExplosionTime() > 10){
                stateMachine.SwitchState(new CrabMonsterExplosionState(stateMachine));
                stateMachine.ResetExplosionTime();
                return;
            }

             if(stateMachine.GetHealTime() > 25){
                stateMachine.SwitchState(new CrabMonsterHealState(stateMachine));
                stateMachine.ResetHealTime();
                return;
            }

            if(isInAttackRange())
            {
                stateMachine.isDetectedPlayed = true;
                stateMachine.SwitchState(new CrabMonsterAttackingState(stateMachine));
                return;
            }
           
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
