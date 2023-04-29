using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantWormAttackingState : GiantWormBaseState
{
    private const float TransitionDuration = 0.1f;

    private string attackChoosed;
    private float timeToWaitEndAnimation;

    public GiantWormAttackingState(GiantWormStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void Enter()
    {
        attackChoosed = GetRandomGiantWormAttack();
        int AttackHash = Animator.StringToHash(attackChoosed);
        
        FacePlayer();
        
        stateMachine.Body.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.Head.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new GiantWormIdleState(stateMachine));
    }

    public override void Tick(float deltaTime){ }

    public override void Exit(){ }

    private string GetRandomGiantWormAttack()
    {
        int num = Random.Range(0,15);
        if(num <= 5 ){
            timeToWaitEndAnimation = 1.3f;
            return "Attack01";
        }else if(num <= 10){
            timeToWaitEndAnimation = 6.05f;
            return "Attack02";
        }else{
            timeToWaitEndAnimation = 6.15f;
            return "Attack03";
        }
    }

}
