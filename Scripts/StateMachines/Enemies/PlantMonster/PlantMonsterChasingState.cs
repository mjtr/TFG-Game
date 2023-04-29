using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMonsterChasingState : PlantMonsterBaseState
{

    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");
    private const float CrossFadeDuration = 0.1f;
    private const float chasingRangeToAdd = 2.1f;
    private int timeToResetNavMesh = 0;

    public PlantMonsterChasingState(PlantMonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        if(!stateMachine.GetIsActionMusicStart())
        {
            stateMachine.StartActionMusic();
        }
        
        if(!stateMachine.isOnlySpellMagic){
            stateMachine.StopAllCourritines();
            stateMachine.StopParticlesEffects();
            stateMachine.DesactiveAllPlantMonsterWeapon();
            stateMachine.SetChasingRange(stateMachine.PlayerChasingRange + chasingRangeToAdd);
            stateMachine.Animator.SetFloat(LocomotionHash, 1f);
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
        }
    }

    public override void Tick(float deltaTime)
    {
        
        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }

        if(isInAttackRange())
        {    
            if(stateMachine.isOnlySpellMagic){
                stateMachine.SwitchState(new PlantMonsterStartMagicState(stateMachine));
                return;
            }

            stateMachine.SwitchState(new PlantMonsterAttackingState(stateMachine));
            return;
        }

        if(!IsInChaseRange())
        {
            if(stateMachine.GetIsActionMusicStart())
            {
                stateMachine.StartAmbientMusic();
            }
            stateMachine.SwitchState(new PlantMonsterShoutState(stateMachine));
            return;
        }

        if(!stateMachine.isOnlySpellMagic){
            MoveToPlayer(deltaTime);
        }

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
            Debug.Log("Resetamos el navMesh de la planta");
            timeToResetNavMesh = 0;
            stateMachine.Agent.ResetPath();
            stateMachine.Agent.enabled = false;
            stateMachine.Agent.enabled = true;
        }
    }

    public override void Exit()
    {
        if(!stateMachine.isOnlySpellMagic){
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
