using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAttackingState : SpiderBaseState
{
    private const float TransitionDuration = 0.1f;
    private float timeToWaitEndAnimation;
    private bool onlyMagic;

    public SpiderAttackingState(SpiderStateMachine stateMachine, bool onlyMagic) : base(stateMachine)
    {
        this.onlyMagic = onlyMagic;
    }
    public override void Enter()
    {   
        string attackChoosed = GetRandomSpiderAttack();
        int AttackHash = Animator.StringToHash(attackChoosed);
        FacePlayer();
        stateMachine.Weapon.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new SpiderChasingState(stateMachine));
    }

    public override void Tick(float deltaTime) {
        stateMachine.AddTimeToScreamTime(deltaTime);    

     }

    public override void Exit(){
        stateMachine.Agent.enabled = true;
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.enabled = false;
        stateMachine.Agent.enabled = true;
    }

    private string GetRandomSpiderAttack()
    {
        if(onlyMagic)
        {
            timeToWaitEndAnimation = 3.7f;
            return "Cast";
        }

        int num = Random.Range(0,15);
        if(num <= 5 ){
            timeToWaitEndAnimation = 1.6f;
            return "Attack 1";

        }else if (num <= 10)
        {
            timeToWaitEndAnimation = 3.7f;
            return "Cast";   

        }else{
            timeToWaitEndAnimation = 2.7f;
            return "Attack 2";
        }
    }

}
