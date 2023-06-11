using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MedusaSerpentineChasingState : MedusaSerpentineBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");
    private const float CrossFadeDuration = 0.1f;
    private int timeToResetNavMesh = 0;
    private bool blockBefore = false;

    public MedusaSerpentineChasingState(MedusaSerpentineStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Agent.enabled = true;
        float chasingRangeToAdd = 10f;
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllMedusaSerpentineWeapon();
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

        stateMachine.AddTimeToScreamTime(deltaTime);    

        if(stateMachine.GetScreamTime() > 6f){
            stateMachine.ResetScreamTime();
            stateMachine.SwitchState(new MedusaSerpentineScreamState(stateMachine));
            return;
        }

        if(isInAttackRange()){
            stateMachine.isDetectedPlayed = true;

            if(stateMachine.GetWarriorPlayerStateMachine().isAttacking)
            {
                if(DodgeAttackRandomize(blockBefore))
                {
                    blockBefore = true;
                    stateMachine.SwitchState(new MedusaSerpentineDodgeState(stateMachine));
                    return;
                }
                blockBefore = false;
                
            }
            stateMachine.SwitchState(new MedusaSerpentineAttackingState(stateMachine));
            return;
        }

    
        if(!IsInChaseRange())
        { 
            stateMachine.isDetectedPlayed = false;
            stateMachine.SetAudioControllerIsAttacking(false);
            stateMachine.StartAmbientMusic();
            if(stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new MedusaSerpentinePatrolPathState(stateMachine));
                return;
            }
            stateMachine.SwitchState(new MedusaSerpentineIdleState(stateMachine));
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

}
