using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DemonChasingState : DemonBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");
    private const float CrossFadeDuration = 0.1f;
    private int timeToResetNavMesh = 0;
    private bool firsTimeToFollowCharater = true;
    private bool blockBefore = false;

    public DemonChasingState(DemonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        float chasingRangeToAdd = 0f;
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDemonWeapon();
        
        if(firsTimeToFollowCharater)
        {
            firsTimeToFollowCharater = false;
            chasingRangeToAdd = 3.1f;
        }
       
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

        if(stateMachine.GetWarriorPlayerStateMachine().isAttacking)
        {
            if(BlockAttackRandomize(blockBefore))
            {
                blockBefore = true;
                stateMachine.isDetectedPlayed = true;
                stateMachine.SwitchState(new DemonBlockState(stateMachine));
                return;
            }
            blockBefore = false;
            
        }

        if(!IsInChaseRange())
        { 
            stateMachine.isDetectedPlayed = false;
            stateMachine.SetAudioControllerIsAttacking(false);
            stateMachine.StartAmbientMusic();
            if(stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new DemonPatrolPathState(stateMachine));
                return;
            }
            stateMachine.SwitchState(new DemonIdleState(stateMachine));
            return;
        
        }else if(isInAttackRange()){
            stateMachine.isDetectedPlayed = true;
            stateMachine.SwitchState(new DemonAttackingState(stateMachine));
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
