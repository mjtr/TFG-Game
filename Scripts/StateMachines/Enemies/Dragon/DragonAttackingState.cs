using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackingState : DragonBaseState
{
    private const float TransitionDuration = 0.1f;
    private string attackChoosed;
    private float timeToWait;
    public DragonAttackingState(DragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {   
        attackChoosed = GetRandomDragonAttack();
        int AttackHash = Animator.StringToHash(attackChoosed);
        
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }


    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWait);
        stateMachine.SwitchState(new DragonChasingState(stateMachine));
    }

    public override void Tick(float deltaTime){ }

    public override void Exit(){ }

    private string GetRandomDragonAttack()
    {
        
        if(isInFrontOfPlayer()){
            stateMachine.WeaponHead.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            FacePlayer();
            timeToWait = 1.15f;
            return "Attack01";
        }

        int num = Random.Range(0,10);
        if(num <= 5){
            stateMachine.WeaponTail.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            stateMachine.WeaponFinishTail.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
             timeToWait = 2.05f;
            return "TailWhipR";
        }else{
            stateMachine.WeaponTail.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            stateMachine.WeaponFinishTail.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
             timeToWait = 2.05f;
            return "TailWhipL";
        }
    
    }

}
