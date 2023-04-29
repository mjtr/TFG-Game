using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomIdleState : MushroomBaseState
{

    private readonly int StatueBlendTreeHash = Animator.StringToHash("Statue");
    private readonly int StatueHash = Animator.StringToHash("statue");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public MushroomIdleState(MushroomStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDemonWeapon();
        stateMachine.Animator.SetFloat(StatueHash, 0f);
        stateMachine.Animator.CrossFadeInFixedTime(StatueBlendTreeHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {

        Move(deltaTime);

        if(IsInChaseRange())
        {
            stateMachine.SwitchState(new MushroomChasingState(stateMachine));
            return;
        }

    }

    public override void Exit(){ }


}
