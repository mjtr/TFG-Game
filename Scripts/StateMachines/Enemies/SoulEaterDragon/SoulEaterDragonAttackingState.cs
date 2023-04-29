using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEaterDragonAttackingState : SoulEaterDragonBaseState
{
    private const float TransitionDuration = 0.1f;

    private string attackChoosed;

    private float timeToWaitEndAnimation;
    public SoulEaterDragonAttackingState(SoulEaterDragonStateMachine stateMachine) : base(stateMachine)    {   }

    public override void Enter()
    {   
        attackChoosed = GetRandomSoulEaterDragonAttack();
        int AttackHash = Animator.StringToHash(attackChoosed);
            
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }


    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new SoulEaterDragonIdleState(stateMachine));
    }

    public override void Tick(float deltaTime){ }

    public override void Exit(){ }

    private string GetRandomSoulEaterDragonAttack()
    {
        
        int num = Random.Range(0,10);
        if(num <= 5 ){
            FacePlayer();
            stateMachine.WeaponHead.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            timeToWaitEndAnimation = 1f;
            return "Basic Attack";
        }
        
        stateMachine.WeaponTail.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        timeToWaitEndAnimation = 1.10f;
        return "Tail Attack";
    }

}
