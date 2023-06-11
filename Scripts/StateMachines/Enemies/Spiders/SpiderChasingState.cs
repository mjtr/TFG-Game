using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderChasingState : SpiderBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private int timeToResetNavMesh = 0;
    private bool blockBefore = false;

    public SpiderChasingState(SpiderStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Agent.enabled = true;
        stateMachine.SetAudioControllerIsAttacking(true);
        stateMachine.StartActionMusic();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllSpiderWeapon();
        stateMachine.Animator.SetFloat(LocomotionHash, 1f);
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        
        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }

             
        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }

        stateMachine.AddTimeToScreamTime(deltaTime);    

        if(stateMachine.GetScreamTime() > 8f){
            stateMachine.ResetScreamTime();
            stateMachine.SwitchState(new SpiderScreamState(stateMachine));
            return;
        }

        if(isInAttackRange()){
            stateMachine.isDetectedPlayed = true;

            if(stateMachine.GetWarriorPlayerStateMachine().isAttacking)
            {
                if(DodgeAttackRandomize(blockBefore))
                {
                    blockBefore = true;
                    stateMachine.SwitchState(new SpiderDodgeState(stateMachine));
                    return;
                }
                blockBefore = false;
                
            }
            stateMachine.SwitchState(new SpiderAttackingState(stateMachine,false));
            return;
        }
        
        if(isInMagicRange())
        {
            stateMachine.SwitchState(new SpiderAttackingState(stateMachine,true));
            return;
        }
    
        if(!IsInChaseRange())
        { 
            stateMachine.isDetectedPlayed = false;
            stateMachine.SetAudioControllerIsAttacking(false);
            stateMachine.StartAmbientMusic();
            if(stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new SpiderPatrolPathState(stateMachine));
                return;
            }
            stateMachine.SwitchState(new SpiderIdleState(stateMachine));
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

    private bool isInMagicRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.MaxMagicRange * stateMachine.MaxMagicRange
            && playerDistanceSqr >= stateMachine.MinMagicRange * stateMachine.MinMagicRange;
    }

}
