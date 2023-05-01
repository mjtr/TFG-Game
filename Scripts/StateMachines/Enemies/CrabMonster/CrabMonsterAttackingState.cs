using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMonsterAttackingState : CrabMonsterBaseState
{
    private const float TransitionDuration = 0.1f;
    private string attackChoosed;
    public CrabMonsterAttackingState(CrabMonsterStateMachine stateMachine) : base(stateMachine)    {   }
    private bool tryCombo = false;
    private float timeToWaitEndAnimation;
    private int countCombo = 0;

    public override void Enter()
    {   
        stateMachine.StopAllCourritines();
        stateMachine.DesactiveAllCrabMonsterWeapon();
        attackChoosed = GetRandomCrabMonsterAttack();
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
            stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(GetRandomCrabMonsterAttackCombo(attackChoosed)), TransitionDuration));
        }else
        {
            stateMachine.SwitchState(new CrabMonsterIdleState(stateMachine));
        }
        
    }

    private void GetTimeToWaitAnimation()
    {
        if(attackChoosed == "Attack_1"){
            timeToWaitEndAnimation = 1.3f;
            stateMachine.RightArmsDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            return;
        }
        
        if(attackChoosed == "Attack_2"){
            timeToWaitEndAnimation = 1.3f;
            stateMachine.RightArmsDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            stateMachine.LeftArmsDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            return;
        }
        
        if(attackChoosed == "Attack_3"){
            timeToWaitEndAnimation = 1.3f;
            stateMachine.RightArmsDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            stateMachine.LeftArmsDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            return;
        }

        if(attackChoosed == "Attack_4"){
            timeToWaitEndAnimation = 1f;
            stateMachine.RightArmsDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            stateMachine.LeftArmsDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            return;
        }

         if(attackChoosed == "Attack_5"){
            timeToWaitEndAnimation = 1.7f;
            stateMachine.HeadDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            return;
        }

    }

    private bool GetRandomTryCombo()
    {

        if(countCombo == 2 || !isInAttackRange()){
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

    public override void Tick(float deltaTime){
        stateMachine.AddTimeToExplosionTime(deltaTime);
        stateMachine.AddTimeToHealTime(deltaTime);
     }

    public override void Exit(){ 
        stateMachine.ResetNavhMesh();
    }

    private string GetRandomCrabMonsterAttack()
    {
        stateMachine.EnableArmsDamage();
        int num = Random.Range(0,25);
        if(num <= 5 ){
            return "Attack_1";

        }else if(num <= 10){
            return "Attack_2";
        
        }else if(num <= 15){
            return "Attack_3";
        }else if(num <= 20){
            return "Attack_4";
        }

       return "Attack_5";
    }

    private string GetRandomCrabMonsterAttackCombo(string firstAttack)
    {
        stateMachine.EnableArmsDamage();
        int num = Random.Range(0,20);
        if(firstAttack == "Attack_1")
        {
            if(num <= 5 ){
                return "Attack_2";
            }

            if(num <= 10 ){
                return "Attack_3";
            }

            if(num <= 15 ){
                return "Attack_4";
            }

            return "Attack_5";

        }

        if(firstAttack == "Attack_2")
        {
            if(num <= 5 ){
                return "Attack_1";
            }

            if(num <= 10 ){
                return "Attack_3";
            }

            if(num <= 15 ){
                return "Attack_4";
            }

            return "Attack_5";

        }

        if(firstAttack == "Attack_3")
        {
            if(num <= 5 ){
                return "Attack_1";
            }

            if(num <= 10 ){
                return "Attack_2";
            }

            if(num <= 15 ){
                return "Attack_4";
            }

            return "Attack_5";
        }

        if(firstAttack == "Attack_4")
        {
            if(num <= 5 ){
                return "Attack_1";
            }

            if(num <= 10 ){
                return "Attack_2";
            }

            if(num <= 15 ){
                return "Attack_3";
            }

            return "Attack_5";
        }

        if(num <= 5 ){
            return "Attack_1";
        }

        if(num <= 10 ){
            return "Attack_2";
        }

        if(num <= 15 ){
            return "Attack_3";
        }
        
        return "Attack_4";

    }
    private bool isInAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
