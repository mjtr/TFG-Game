using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockMonsterChasingState : RockMonsterBaseState
{

    private readonly int WalkHash = Animator.StringToHash("Walk");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private int timeToResetNavMesh = 0;
    private bool firsTimeToFollowCharater = true;
    public RockMonsterChasingState(RockMonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        float chasingRangeToAdd = 0f;
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllRockMonsterWeapon();
        stateMachine.StopSounds();
        
        if(firsTimeToFollowCharater)
        {
            firsTimeToFollowCharater = false;
            chasingRangeToAdd = 1.5f;
        }

        if(!stateMachine.GetIsActionMusicStart())
        {
            stateMachine.StartActionMusic();
        }

        stateMachine.SetChasingRange(stateMachine.PlayerChasingRange + chasingRangeToAdd);
        stateMachine.Animator.CrossFadeInFixedTime(WalkHash, CrossFadeDuration);
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
                
                stateMachine.SwitchState(new RockMonsterBlockState(stateMachine));
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
            stateMachine.SwitchState(new RockMonsterPatrolPathState(stateMachine));
            return;
        
        }else if(isInAttackRange()){
            stateMachine.isDetectedPlayed = true;
            stateMachine.SwitchState(new RockMonsterAttackingState(stateMachine));
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

    private bool isInFireBreathAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.FireBallAttackRange * stateMachine.FireBallAttackRange;
    }

}
