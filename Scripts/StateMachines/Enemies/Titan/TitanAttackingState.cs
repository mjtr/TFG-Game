using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanAttackingState : TitanBaseState
{
    private const float TransitionDuration = 0.1f;
    private string attackChoosed;
    public TitanAttackingState(TitanStateMachine stateMachine) : base(stateMachine)    {   }
    private bool tryCombo = false;
    private float timeToWaitEndAnimation;
    private int countCombo = 0;

    public override void Enter()
    {   
        stateMachine.StopAllCourritines();
        stateMachine.DesactiveAllTitanWeapon();
        attackChoosed = GetRandomTitanAttack();
        tryCombo = GetRandomTryCombo();
        int AttackHash = Animator.StringToHash(attackChoosed);
        FacePlayer();
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        GetTimeToWaitAnimation();
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        if(tryCombo)
        {
            tryCombo = GetRandomTryCombo();
            FacePlayer();
            stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(GetRandomTitanAttackCombo(attackChoosed)), TransitionDuration));
        }else
        {
            stateMachine.SwitchState(new TitanIdleState(stateMachine));
        }
        
    }

    private void GetTimeToWaitAnimation()
    {
        if(attackChoosed == "Attack1"){
            timeToWaitEndAnimation = 2.3f;
            stateMachine.RightArmsDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            return;
        }
        
        if(attackChoosed == "Attack2"){
            timeToWaitEndAnimation = 2.74f;
            stateMachine.RightArmsDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            stateMachine.LeftArmsDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            return;
        }
        
        if(attackChoosed == "Attack3"){
            timeToWaitEndAnimation = 5f;
            stateMachine.RightArmsDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            stateMachine.LeftArmsDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            return;
        }

    }

    private bool GetRandomTryCombo()
    {

        if(countCombo == 2 || attackChoosed == "Attack3"|| !isInAttackRange()){
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

    private string GetRandomTitanAttack()
    {
        stateMachine.EnableArmsDamage();
        int num = Random.Range(0,15);
        if(num <= 5 ){
            return "Attack1";

        }else if(num <= 10){
            return "Attack2";
        }

       return "Attack3";
    }

    private string GetRandomTitanAttackCombo(string firstAttack)
    {
        stateMachine.EnableArmsDamage();
        int num = Random.Range(0,10);
        if(firstAttack == "Attack1")
        {
            if(num <= 5 ){
                return "Attack2";
            }
            return "Attack3";

        }

        if(firstAttack == "Attack2")
        {
            if(num <= 5 ){
                return "Attack1";
            }
            
            return "Attack3";
        }

        if(num <= 5 ){
            return "Attack1";
        }
        
        return "Attack3";

    }
    private bool isInAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
