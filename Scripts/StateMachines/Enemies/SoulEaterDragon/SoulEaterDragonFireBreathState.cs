using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEaterDragonFireBreathState : SoulEaterDragonBaseState
{
    private const float TransitionDuration = 0.1f;
    private string attackChoosed = "Fireball Shoot";
    public SoulEaterDragonFireBreathState(SoulEaterDragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {   
        //stateMachine.SoulEaterDragonFirebreath.FireBreathWeaponLogic.GetComponent<WeaponDamage>().SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        int AttackHash = Animator.StringToHash(attackChoosed);
        
        //Antes de atacar girará 
        FacePlayer();
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }


    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(stateMachine.Animator.GetCurrentAnimatorStateInfo(0).length + 0.5f);
        stateMachine.SwitchState(new SoulEaterDragonIdleState(stateMachine));
    }

    public override void Tick(float deltaTime){ }

    public override void Exit(){ }

  

}
