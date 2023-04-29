using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilAttackingState : DevilBaseState
{
    private const float TransitionDuration = 0.1f;

    private string attackChoosed;
    public DevilAttackingState(DevilStateMachine stateMachine) : base(stateMachine)    {   }

    private bool tryCombo = false;

    private float timeToWaitEndAnimation;

    private int countCombo = 0;

    public override void Enter()
    {   
        stateMachine.StopAllCourritines();
        stateMachine.StopSounds();
        stateMachine.DesactiveAllDevilWeapon();
        attackChoosed = GetRandomDevilAttack();
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
            stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(GetRandomDevilAttackCombo(attackChoosed)), TransitionDuration));
        }else
        {
            stateMachine.SwitchState(new DevilIdleState(stateMachine));
        }
        
    }

    private void GetTimeToWaitAnimation()
    {
        if(attackChoosed == "attack1"){
            timeToWaitEndAnimation = 1.45f;
            return;
        }
        
        if(attackChoosed == "attack2"){
            timeToWaitEndAnimation = 1.45f;
            return;
        }
        
        if(attackChoosed == "attack3"){
            timeToWaitEndAnimation = 1.67f;
            return;
        }
        
        if(attackChoosed == "attack4"){
            timeToWaitEndAnimation = 1.67f;
            return;
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

    private string GetRandomDevilAttack()
    {
        int num = Random.Range(0,20);
        if(num <= 5 ){
            stateMachine.EnableArmsDamage();
            return "attack1";

        }else if(num <= 10){
            stateMachine.EnableArmsDamage();
            return "attack2";

        }else if(num <= 15){
            stateMachine.HeadDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            return "attack3";
        }
       stateMachine.EnableArmsDamage();       
       return "attack4";
    }

    private string GetRandomDevilAttackCombo(string firstAttack)
    {
        int num = Random.Range(0,15);
        if(firstAttack == "attack1")
        {
            if(num <= 5 ){
                stateMachine.EnableArmsDamage();
                return "attack2";
            }
            
            if(num <= 10 ){
                stateMachine.HeadDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
                return "attack3";
            }

            if(num <= 15 ){
                stateMachine.EnableArmsDamage();
                return "attack4";
            }
        }

        if(firstAttack == "attack2")
        {
            if(num <= 5 ){
                stateMachine.EnableArmsDamage();
                return "attack1";
            }
            
            if(num <= 10 ){
                stateMachine.HeadDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
                return "attack3";
            }

            if(num <= 15 ){
                stateMachine.EnableArmsDamage();
                return "attack4";
            }
        }

        if(firstAttack == "attack3")
        {
            stateMachine.EnableArmsDamage();
            if(num <= 5 ){
                return "attack1";
            }
            
            if(num <= 10 ){
                return "attack2";
            }

            if(num <= 15 ){
                return "attack4";
            }
        }

        
        if(num <= 5 ){
            stateMachine.EnableArmsDamage();
            return "attack1";
        }
        
        if(num <= 10 ){
            stateMachine.EnableArmsDamage();
            return "attack2";
        }

        stateMachine.HeadDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        return "attack3";

    }
    private bool isInAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
