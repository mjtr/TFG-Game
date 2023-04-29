using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomChasingState : MushroomBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");

    private readonly int LocomotionHash = Animator.StringToHash("locomotion");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    private const float chasingRangeToAdd = 6.1f;

    private int timeToResetNavMesh = 0;

    public MushroomChasingState(MushroomStateMachine stateMachine) : base(stateMachine)
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
        stateMachine.DesactiveAllDemonWeapon();
        stateMachine.SetChasingRange(stateMachine.PlayerChasingRange + chasingRangeToAdd);
        stateMachine.Animator.SetFloat(LocomotionHash, 1f);
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        
        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }

        if(isInAttackRange()){
            stateMachine.SwitchState(new MushroomAttackingState(stateMachine,false));
            return;
        }

        if(isInMagicRange())
        {
            stateMachine.SwitchState(new MushroomAttackingState(stateMachine,true));
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

    private bool isInMagicRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.MaxMagicRange * stateMachine.MaxMagicRange
            && playerDistanceSqr >= stateMachine.MinMagicRange * stateMachine.MinMagicRange;
    }

}
