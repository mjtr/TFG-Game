using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrorDragonAttackingState : TerrorDragonBaseState
{
    private const float TransitionDuration = 0.1f;

    private string attackChoosed;

    private float timeToWaitEndAnimation;
    public TerrorDragonAttackingState(TerrorDragonStateMachine stateMachine) : base(stateMachine)    {   }

    public override void Enter()
    {   
        attackChoosed = GetRandomTerrorDragonAttack();
        int AttackHash = Animator.StringToHash(attackChoosed);
            
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new TerrorDragonIdleState(stateMachine));
    }

    public override void Tick(float deltaTime){
        stateMachine.AddTimeToFlyTime(deltaTime);
        stateMachine.AddTimeToScreamTime(deltaTime);      
    }

    public override void Exit(){ }

    private string GetRandomTerrorDragonAttack()
    {
        
        int num = Random.Range(0,10);
        if(num <= 5 ){
            FacePlayer();
            stateMachine.WeaponHead.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            timeToWaitEndAnimation = 1f;
            return "Basic Attack";
        }
        
        stateMachine.WeaponLefttWing.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.WeaponRightWing.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        timeToWaitEndAnimation = 1.4f;
        return "Claw Attack";
    }

}
