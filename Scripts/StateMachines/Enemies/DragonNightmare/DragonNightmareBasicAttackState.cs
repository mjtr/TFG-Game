using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonNightmareBasicAttackState : DragonNightmareBaseState
{
    private const float TransitionDuration = 0.1f;

    private string attackChoosed;
    private float timeToWaitEndAnimation = 1.17f;
    public DragonNightmareBasicAttackState(DragonNightmareStateMachine stateMachine) : base(stateMachine)    {   }

    public override void Enter()
    {   
        stateMachine.StopAllCourritines();
        stateMachine.DesactiveAllDragonNightmareWeapon();
        attackChoosed = GetRandomDragonAttack();
        int AttackHash = Animator.StringToHash(attackChoosed);
        FacePlayer();
        stateMachine.HeadDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new DragonNightmareChasingState(stateMachine));
        
    }

     private string GetRandomDragonAttack()
    {
       
        int num = Random.Range(0,15);
        if(num <= 5){
            stateMachine.HeadDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            timeToWaitEndAnimation = 2.2f;
            return "Horn Attack";
        }else if (num <= 10){
            stateMachine.HeadDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            timeToWaitEndAnimation = 1.3f;
            return "Basic Attack";
        }else
        {
            timeToWaitEndAnimation = 3.4f;
            stateMachine.ArmRightDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            return "Claw Attack";
        }
    
    }

    public override void Tick(float deltaTime){ }

    public override void Exit(){ }
 
}
