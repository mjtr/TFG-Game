using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMonsterIdleState : RockMonsterBaseState
{

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public RockMonsterIdleState(RockMonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopSounds();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllRockMonsterWeapon();
        int IdleHash = getRandomIdleHash();
        stateMachine.Animator.CrossFadeInFixedTime(IdleHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        
        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }

        Move(deltaTime);

        if(!IsInChaseRange() && stateMachine.PatrolPath != null)
        {
            stateMachine.isDetectedPlayed = false;
            stateMachine.SwitchState(new RockMonsterPatrolPathState(stateMachine));
            return;
        }

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            stateMachine.isDetectedPlayed = true;
            stateMachine.SwitchState(new RockMonsterChasingState(stateMachine));
            return;
        }
        
        stateMachine.isDetectedPlayed = false;
    }

    private int getRandomIdleHash()
    {
        int num = Random.Range(0,10);
        if(num <= 5 ){
            return Animator.StringToHash("Idle");
        }
       return Animator.StringToHash("IdleBreak");
    }

    public override void Exit(){
        stateMachine.ResetNavMesh();

     }


}
