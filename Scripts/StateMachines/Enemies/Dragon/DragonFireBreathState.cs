using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFireBreathState : DragonBaseState
{
    private const float TransitionDuration = 0.1f;
    private string attackChoosed = "BreatheFire";

    private float timeToWaitEndAnimation = 2.8f;
    public DragonFireBreathState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {   
        stateMachine.DragonFirebreath.FireBreathWeaponLogic.GetComponent<WeaponDamage>().SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        int AttackHash = Animator.StringToHash(attackChoosed);
        
        FacePlayer();
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }


    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new DragonChasingState(stateMachine));
    }

    public override void Tick(float deltaTime){ }

    public override void Exit(){ }

  

}
