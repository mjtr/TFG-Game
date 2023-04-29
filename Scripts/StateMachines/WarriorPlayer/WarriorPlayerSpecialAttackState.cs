using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

public class WarriorPlayerSpecialAttackState : WarriorPlayerBaseState
{
    
    private readonly int CastingHash = Animator.StringToHash("Casting");
    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 5.5f;

    public WarriorPlayerSpecialAttackState(WarriorPlayerStateMachine stateMachine) : base(stateMachine) {}
    public override void Enter()
    {
        float staminaConsumed = stateMachine.Health.GetActualMaxStaminaPoints() * 0.75f;
        stateMachine.Stamina.TakeStamina(staminaConsumed);
        stateMachine.StartCoroutine(WaitForAnimationToEnd(CastingHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);

         if(stateMachine.Targeter.CurrentTarget != null){
                stateMachine.SwitchState(new WarriorPlayerTargetingState(stateMachine));
            }else{
                stateMachine.SwitchState(new WarriorPlayerFreeLookState(stateMachine));
            }
    }

    public override void Tick(float deltaTime)
    { }

    public override void Exit()
    { 
       
    }

}
