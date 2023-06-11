using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrorDragonChasingState : TerrorDragonBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");
    private const float CrossFadeDuration = 0.1f;
    private const float chasingRangeToAdd = 0f;

    private int timeToResetNavMesh = 0;

    public TerrorDragonChasingState(TerrorDragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Agent.enabled = true;
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllTerrorDragonWeapon();
        
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
        
        stateMachine.AddTimeToFlyTime(deltaTime);        
        stateMachine.AddTimeToScreamTime(deltaTime);    

        if(stateMachine.GetScreamTime() > 6f){
            stateMachine.ResetScreamTime();
            stateMachine.SwitchState(new TerrorDragonScreamState(stateMachine));
            return;
        }

        if(stateMachine.GetFlyTime() > 12f){
            stateMachine.ResetFlyTime();
            stateMachine.SwitchState(new TerrorDragonStartFlyingState(stateMachine));
            return;
        }

        if(isInAttackRange())
        {
            if(stateMachine.GetWarriorPlayerStateMachine().isAttacking)
            {
                stateMachine.isDetectedPlayed = true;

                if(BlockAttackRandomize())
                {
                    stateMachine.SwitchState(new TerrorDragonStartBlockingState(stateMachine));
                    return;
                }
            }

            stateMachine.SwitchState(new TerrorDragonAttackingState(stateMachine));
            return;
        }  

        if(isInMagicRange())
        {
            stateMachine.SwitchState(new TerrorDragonFireBreathState(stateMachine));
            return;
        }

        
        if(!IsInChaseRange())
        {
            stateMachine.SetAudioControllerIsAttacking(false);
            stateMachine.StartAmbientMusic();
            
            stateMachine.isDetectedPlayed = false;
            if(stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new TerrorDragonPatrolPathState(stateMachine));
                return;
            }
            stateMachine.SwitchState(new TerrorDragonIdleState(stateMachine));
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
           stateMachine.ResetNavhMesh();
           stateMachine.Agent.velocity = Vector3.zero;
        }
    }

    private bool isInAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }

    private bool isInMagicRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.MaxMagicRange * stateMachine.MaxMagicRange
            && playerDistanceSqr >= stateMachine.MinMagicRange * stateMachine.MinMagicRange;
    }


}
