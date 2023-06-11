using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaSerpentineAttackingState : MedusaSerpentineBaseState
{
    private const float TransitionDuration = 0.1f;
    private string attackChoosed;
    public MedusaSerpentineAttackingState(MedusaSerpentineStateMachine stateMachine) : base(stateMachine)    {   }
    private bool tryCombo = false;
    private int countCombo = 0;
    private float timeToWaitEndAnimation;

    public override void Enter()
    {   
        attackChoosed = GetRandomMedusaSerpentineAttack();
        tryCombo = GetRandomTryCombo();
        int AttackHash = Animator.StringToHash(attackChoosed);
        FacePlayer();
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {   
        bool attackFastter = AttackFastter();
        getTimeToWaitEndAnimation();
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
            attackChoosed = GetRandomMedusaSerpentineAttackCombo(attackChoosed);
            stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(attackChoosed), TransitionDuration));
        }else
        {
            stateMachine.SwitchState(new MedusaSerpentineChasingState(stateMachine));
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

    public override void Tick(float deltaTime){ 
        stateMachine.AddTimeToScreamTime(deltaTime);    
    }

    public override void Exit(){ 
        stateMachine.ResetNavMesh();
        stateMachine.Animator.SetFloat("Speed", 1.0f);
    }

    private string GetRandomMedusaSerpentineAttack()
    {
        stateMachine.WeaponSwordDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        int num = Random.Range(0,20);
        if(num <= 5 ){
            return "Attack1";

        }else if(num <= 10){
            return "Attack2";

        }
       return "Attack3";
    }

    private string GetRandomMedusaSerpentineAttackCombo(string firstAttack)
    {
        stateMachine.WeaponSwordDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
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
        
        return "Attack2";
    }
    private bool isInAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }

    private void getTimeToWaitEndAnimation(){
        if(attackChoosed == "Attack1"){
            timeToWaitEndAnimation = 1.9f;
        }else if(attackChoosed == "Attack2"){
            timeToWaitEndAnimation = 1.5f;
        }else {
            timeToWaitEndAnimation = 1.7f;
        }
    }
}
