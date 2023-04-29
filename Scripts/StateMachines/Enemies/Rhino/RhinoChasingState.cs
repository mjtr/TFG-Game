using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoChasingState : RhinoBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");

    private readonly int SpeedHash = Animator.StringToHash("locomotion");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private int timeToResetNavMesh = 0;

    private bool isFirstTime = true;

    public RhinoChasingState(RhinoStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
    
        if(!stateMachine.GetIsActionMusicStart())
        {
            stateMachine.StartActionMusic();
        }

        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllRhinoWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {

        if(!IsInChaseRange())
        {

            if(stateMachine.GetIsActionMusicStart())
            {
                stateMachine.StartAmbientMusic();
            }
            
            stateMachine.isDetectedPlayed = false;
            if(stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new RhinoPatrolPathState(stateMachine));
                return; 
            }
            return; 
        
        }else if(isInAttackRange()){
            stateMachine.isDetectedPlayed = true;
            stateMachine.SwitchState(new RhinoAttackingState(stateMachine));
            return;
        }

        MoveToPlayer(deltaTime);

        FacePlayer();

        stateMachine.Animator.SetFloat(SpeedHash,1f, AnimatorDampTime, deltaTime);
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
