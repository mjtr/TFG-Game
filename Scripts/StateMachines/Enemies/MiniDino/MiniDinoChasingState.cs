using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiniDinoChasingState : MiniDinoBaseState
{

    private readonly int WalkHash = Animator.StringToHash("Walk");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private int timeToResetNavMesh = 0;
    private bool firsTimeToFollowCharater = true;

    private int blockingNumber = 0;
    public MiniDinoChasingState(MiniDinoStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Agent.enabled = true;
        float chasingRangeToAdd = 0f;
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllMiniDinoWeapon();
        
        if(firsTimeToFollowCharater)
        {
            firsTimeToFollowCharater = false;
            chasingRangeToAdd = 3.1f;
        }

        stateMachine.StartActionMusic();
        stateMachine.SetAudioControllerIsAttacking(true);
        stateMachine.SetChasingRange(stateMachine.PlayerChasingRange + chasingRangeToAdd);
        stateMachine.Animator.CrossFadeInFixedTime(WalkHash, CrossFadeDuration);
        stateMachine.isDetectedPlayed = true;
    }

    public override void Tick(float deltaTime)
    {
        
        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }

        if(isInAttackRange()){
            stateMachine.isDetectedPlayed = true;

            if(stateMachine.GetWarriorPlayerStateMachine().isAttacking)
            {
                if(BlockAttackRandomize())
                {
                    if(blockingNumber < 2)
                    {
                        blockingNumber ++;
                        stateMachine.SwitchState(new MiniDinoBlockState(stateMachine));
                        return;
                    }else{
                        blockingNumber = 0;
                    }
                
                }
            }

            stateMachine.SwitchState(new MiniDinoAttackingState(stateMachine));
            return;
        }
        

        if(!IsInChaseRange())
        {
            stateMachine.SetAudioControllerIsAttacking(false);
            stateMachine.StartAmbientMusic();
            stateMachine.isDetectedPlayed = false;
            stateMachine.SwitchState(new MiniDinoPatrolPathState(stateMachine));
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
            Debug.Log("Resetamos el navMesh del MiniDino");
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
