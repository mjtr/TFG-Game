using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantWormIdleState : GiantWormBaseState
{

    private readonly int IdleHash = Animator.StringToHash("Idle");
    private const float CrossFadeDuration = 0.1f;
    public GiantWormIdleState(GiantWormStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.StopWormSounds();
        stateMachine.DesactiveAllWormWeapon();
        stateMachine.StopParticlesEffects();
        stateMachine.StopAllCoroutines();
        stateMachine.Animator.CrossFadeInFixedTime(IdleHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        
        if(stateMachine.PlayerHealth.CheckIsDead()){ return; }

        FacePlayer();            
        if(isInAttackRange()){
            stateMachine.SwitchState(new GiantWormAttackingState(stateMachine));
            return;
        }else if(!IsInChaseRange()){
            stateMachine.SwitchState(new GiantWormDisappearState(stateMachine));
            return;
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

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - (stateMachine.BoneToCalculateAttackRange.transform.position)).sqrMagnitude;

        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }

}
