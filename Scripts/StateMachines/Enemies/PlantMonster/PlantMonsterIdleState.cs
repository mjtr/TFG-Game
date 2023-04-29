using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMonsterIdleState : PlantMonsterBaseState
{

    private readonly int IdleHash = Animator.StringToHash("IdlePlant");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public PlantMonsterIdleState(PlantMonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllPlantMonsterWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(IdleHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        
        if(!stateMachine.isOnlySpellMagic){
            Move(deltaTime);
        }

        if(IsInChaseRange())
        {
            stateMachine.SwitchState(new PlantMonsterChasingState(stateMachine));
            return;
        }

    }

    public override void Exit(){ }


}
