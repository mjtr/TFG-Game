using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollAttackingState : TrollBaseState
{
    private const float TransitionDuration = 0.1f;
    private string attackChoosed;
    private bool tryCombo = false;
    private float timeToWaitEndAnimation;
    private int countCombo = 0;

    public TrollAttackingState(TrollStateMachine stateMachine) : base(stateMachine)    {   }

    public override void Enter()
    {   
        stateMachine.StopAllCourritines();
        stateMachine.DesactiveAllTrollWeapon();
        attackChoosed = GetRandomTrollAttack();
        tryCombo = GetRandomTryCombo();
        int AttackHash = Animator.StringToHash(attackChoosed);
        FacePlayer();
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
      
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        if(tryCombo)
        {
            tryCombo = GetRandomTryCombo();
            FacePlayer();
            stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(GetRandomTrollAttackCombo(attackChoosed)), TransitionDuration));
        }else
        {
            stateMachine.SwitchState(new TrollIdleState(stateMachine));
        }
        
    }

    private bool GetRandomTryCombo()
    {
        if(!isInAttackRange()){return false;}

        if(countCombo == 2){
            countCombo = 0;
            return false;
        }
        countCombo ++;
        int num = Random.Range(0,20);
        if(num <= 7 ){
            return true;
        }
        return false;
    }

    public override void Tick(float deltaTime){ }

    public override void Exit(){
        stateMachine.ResetNavhMesh();
    }

    private string GetRandomTrollAttack()
    {
        stateMachine.WeaponDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        int num = Random.Range(0,15);
        if(num <= 5 ){
            timeToWaitEndAnimation = 2.9f;
            return "Attack1";

        }else if(num <= 10){
            timeToWaitEndAnimation = 2.8f;
            return "Attack2";

        }else {
            timeToWaitEndAnimation = 2.6f;
            return "Attack3";
        }
    }

    private string GetRandomTrollAttackCombo(string firstAttack)
    {
        stateMachine.WeaponDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        int num = Random.Range(0,10);
        if(firstAttack == "Attack1")
        {
            if(num <= 5 ){
                timeToWaitEndAnimation = 2.8f;
                return "Attack2";
            }
            timeToWaitEndAnimation = 2.6f;
            return "Attack3";
            
        }

        if(firstAttack == "Attack2")
        {
            if(num <= 5 ){
                timeToWaitEndAnimation = 2.9f;
                return "Attack1";
            }
            timeToWaitEndAnimation = 2.6f;
            return "Attack3";  
        }

        if(num <= 5 ){
            timeToWaitEndAnimation = 2.9f;
            return "Attack1";
        }
        timeToWaitEndAnimation = 2.8f;
        return "Attack2";

    }
    private bool isInAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
