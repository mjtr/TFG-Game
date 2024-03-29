using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonAttackingState : DemonBaseState
{
    private const float TransitionDuration = 0.2f;
    private string attackChoosed;
    public DemonAttackingState(DemonStateMachine stateMachine) : base(stateMachine)    {   }
    private bool tryCombo = false;
    private int countCombo = 0;
    private float timeToWaitEndAnimation;

    public override void Enter()
    {   
        attackChoosed = GetRandomDemonAttack();
        tryCombo = GetRandomTryCombo();
        int AttackHash = Animator.StringToHash(attackChoosed);
        FacePlayer();
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {   
        bool attackFastter = AttackFastter();

        if(attackFastter){
            stateMachine.Animator.SetFloat("Speed", 1.25f);
            timeToWaitEndAnimation = timeToWaitEndAnimation / 1.25f;
        }

        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);

        yield return new WaitForSeconds(timeToWaitEndAnimation);

        if(attackFastter){
            stateMachine.Animator.SetFloat("Speed", 1.0f);
        }
        

        if(tryCombo)
        {
            tryCombo = GetRandomTryCombo();
            FacePlayer();
            attackChoosed = GetRandomDemonAttackCombo(attackChoosed);
            stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(attackChoosed), TransitionDuration));
        }else
        {
            stateMachine.SwitchState(new DemonChasingState(stateMachine));
        }
        
    }

    private bool AttackFastter()
    {
        int num = Random.Range(0,20);
        if(num <= 7 ){
            return true;
        }
        return false;
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
        stateMachine.ResetNavMesh();
    }

    private string GetRandomDemonAttack()
    {
        stateMachine.WeaponSwordDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        int num = Random.Range(0,20);
        if(num <= 5 ){
            timeToWaitEndAnimation = 2f;
            return "Attack1";

        }else if(num <= 10){
            timeToWaitEndAnimation = 2f;
            return "Attack2";

        }else if(num <= 15){
            timeToWaitEndAnimation = 1.25f;
            return "Attack3";
        }
        timeToWaitEndAnimation = 2.05f;
       return "Attack4";
    }

    private string GetRandomDemonAttackCombo(string firstAttack)
    {
        stateMachine.WeaponSwordDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        int num = Random.Range(0,15);
        if(firstAttack == "Attack1")
        {
            if(num <= 5 ){
                timeToWaitEndAnimation = 2f;
                return "Attack2";
            }
            
            if(num <= 10 ){
                timeToWaitEndAnimation = 1.25f;
                return "Attack3";
            }

            timeToWaitEndAnimation = 2.05f;
            return "Attack4";
            
        }

        if(firstAttack == "Attack2")
        {
            if(num <= 5 ){
                timeToWaitEndAnimation = 2f;
                return "Attack1";
            }
            
            if(num <= 10 ){
                timeToWaitEndAnimation = 1.25f;
                return "Attack3";
            }
            timeToWaitEndAnimation = 2.05f;
            return "Attack4";
            
        }

        if(firstAttack == "Attack3")
        {
            if(num <= 5 ){
                timeToWaitEndAnimation = 2f;
                return "Attack1";
            }
            
            if(num <= 10 ){
                timeToWaitEndAnimation = 2f;
                return "Attack2";
            }
            timeToWaitEndAnimation = 2.05f;
            return "Attack4";
            
        }

        
        if(num <= 5 ){
            timeToWaitEndAnimation = 2f;
            return "Attack1";
        }
        
        if(num <= 10 ){
            timeToWaitEndAnimation = 2f;
            return "Attack2";
        }
        timeToWaitEndAnimation = 1.25f;
        return "Attack3";

    }
    private bool isInAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
