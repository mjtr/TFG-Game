using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DragonUsurperChasingState : DragonUsurperBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private int timeToResetNavMesh = 0;
    private bool firsTimeToFollowCharater = true;
    public DragonUsurperChasingState(DragonUsurperStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDragonUsurperWeapon();
        
        if(firsTimeToFollowCharater)
        {
            firsTimeToFollowCharater = false;
            stateMachine.SetChasingRange(stateMachine.PlayerChasingRange + 5f);            
        }

        if(!stateMachine.GetIsActionMusicStart())
        {
            stateMachine.StartActionMusic();
        }

        stateMachine.Animator.SetFloat(LocomotionHash, 1f);
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
        stateMachine.isDetectedPlayed = true;
    }

    public override void Tick(float deltaTime)
    {
        
        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }

        if(stateMachine.GetWarriorPlayerStateMachine().isAttacking)
        {
            if(BlockAttackRandomize())
            {
                stateMachine.isDetectedPlayed = true;
                
                stateMachine.SwitchState(new DragonUsurperBlockState(stateMachine));
                return;
            }
            
        }

        if(!IsInChaseRange())
        {
            if(stateMachine.GetIsActionMusicStart())
            {
                stateMachine.StartAmbientMusic();
            }
            stateMachine.isDetectedPlayed = false;
            stateMachine.SwitchState(new DragonUsurperPatrolPathState(stateMachine));
            return;
        
        }
        else if(isInBasicAttackRange()){
            stateMachine.isDetectedPlayed = true;
            stateMachine.SwitchState(new DragonUsurperBasicAttackState(stateMachine));
            return;
        }
        else{
            
            bool isClawRange = isInClawAttackRange();
            bool isFireBreathRange = isInFireBreathAttackRange();

            if(!isClawRange && isFireBreathRange)
            {
                stateMachine.isDetectedPlayed = true;
                stateMachine.SwitchState(new DragonUsurperFireBreathState(stateMachine));
                
                return;
            }

            if(isClawRange && !isFireBreathRange)
            {
                stateMachine.isDetectedPlayed = true;
                stateMachine.SwitchState(new DragonUsurperClawAttackState(stateMachine));
                return;
            }

            if(isClawRange && !isFireBreathRange)
            {
                stateMachine.isDetectedPlayed = true;
                int num = Random.Range(0,20);
                if(num <= 10){
                    stateMachine.SwitchState(new DragonUsurperFireBreathState(stateMachine));
                }else{
                    stateMachine.SwitchState(new DragonUsurperClawAttackState(stateMachine));
                }   
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

    private bool isInClawAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.ClawAttackRange * stateMachine.ClawAttackRange;
    }

    private bool isInFireBreathAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.MaxFireBreathAttackRange * stateMachine.MaxFireBreathAttackRange 
            && playerDistanceSqr >= stateMachine.MinFireBreathAttackRange * stateMachine.MinFireBreathAttackRange ;
    }

}
