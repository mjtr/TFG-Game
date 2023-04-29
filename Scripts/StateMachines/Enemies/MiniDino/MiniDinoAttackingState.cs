using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniDinoAttackingState : MiniDinoBaseState
{
    private const float TransitionDuration = 0.1f;

    private string attackChoosed;
    public MiniDinoAttackingState(MiniDinoStateMachine stateMachine) : base(stateMachine)    {   }

    public override void Enter()
    {   
        attackChoosed = GetRandomMiniDinoAttack();
        int AttackHash = Animator.StringToHash(attackChoosed);
        FacePlayer();
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(stateMachine.Animator.GetCurrentAnimatorStateInfo(0).length + 0.5f);
        stateMachine.SwitchState(new MiniDinoIdleState(stateMachine));
    }

    public override void Tick(float deltaTime){ }

    public override void Exit(){ }

    private string GetRandomMiniDinoAttack()
    {
        
        int num = Random.Range(0,10);
        if(num <= 5 ){
            stateMachine.WeaponHead.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            return "Attack1";
        }
        stateMachine.WeaponLeg.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
       return "Attack2";
    }

}
