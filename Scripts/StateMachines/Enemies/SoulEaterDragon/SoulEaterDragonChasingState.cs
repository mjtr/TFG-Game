using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEaterDragonChasingState : SoulEaterDragonBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");

    private readonly int LocomotionHash = Animator.StringToHash("locomotion");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    private const float chasingRangeToAdd = 0f;

    private int timeToResetNavMesh = 0;

    public SoulEaterDragonChasingState(SoulEaterDragonStateMachine stateMachine) : base(stateMachine)
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
        stateMachine.AddTimeToFlyTime(deltaTime);        

        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }
        
        if(stateMachine.GetFlyTime() > 10f){
            stateMachine.ResetFlyTime();
            stateMachine.SwitchState(new SoulEaterDragonStartFlyingState(stateMachine));
            return;
        }

        if(isInAttackRange())
        {
            if(stateMachine.GetWarriorPlayerStateMachine().isAttacking)
            {
                stateMachine.isDetectedPlayed = true;

                if(BlockAttackRandomize())
                {
                    stateMachine.SwitchState(new SoulEaterDragonStartBlockingState(stateMachine));
                    return;
                }
            }

            stateMachine.SwitchState(new SoulEaterDragonAttackingState(stateMachine));
            return;
        }  

        if(stateMachine.canThrowFireBall && isInMagicRange())
        {
            stateMachine.SwitchState(new SoulEaterDragonFireBreathState(stateMachine));
            return;
        }

        
        if(!IsInChaseRange())
        {
            stateMachine.SetAudioControllerIsAttacking(false);
            stateMachine.StartAmbientMusic();
            
            stateMachine.isDetectedPlayed = false;
            if(stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new SoulEaterDragonPatrolPathState(stateMachine));
                return;
            }
            stateMachine.SwitchState(new SoulEaterDragonIdleState(stateMachine));
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
            Debug.Log("Resetamos el navMesh del SoulEaterDragon");
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

    private bool isInMagicRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.MaxMagicRange * stateMachine.MaxMagicRange
            && playerDistanceSqr >= stateMachine.MinMagicRange * stateMachine.MinMagicRange;
    }


}
