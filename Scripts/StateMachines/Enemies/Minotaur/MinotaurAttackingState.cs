using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurAttackingState : MinotaurBaseState
{
    private const float TransitionDuration = 0.2f;
    private string attackChoosed;
    public MinotaurAttackingState(MinotaurStateMachine stateMachine) : base(stateMachine)    {   }
    private bool tryCombo = false;
    private int countCombo = 0;
    private float timeToWaitEndAnimation;

    public override void Enter()
    {   
        attackChoosed = GetRandomMinotaurAttack();
        tryCombo = GetRandomTryCombo();
        bool trySuperCombo = GetRandomTrySuperCombo();
        int AttackHash = Animator.StringToHash(attackChoosed);
        FacePlayer();
        if(trySuperCombo){
            stateMachine.StartCoroutine(WaitForSuperComboEnd(TransitionDuration));
        }else{
            stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
        }
    }

    private bool GetRandomTrySuperCombo()
    {
        if(!isInAttackRange()){return false;}

        if(countCombo == 2){
            countCombo = 0;
            return false;
        }
        countCombo ++;
        int num = Random.Range(0,20);
        if(num <= 2 ){
            return true;
        }
        return false;
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {   
        bool attackFastter = AttackFastter();
        getTimeToWaitEndAnimation();
        if(attackFastter){
            stateMachine.Animator.SetFloat("speed", 1.25f);
            timeToWaitEndAnimation = timeToWaitEndAnimation / 1.25f;
        }

        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);

        yield return new WaitForSeconds(timeToWaitEndAnimation);

        if(attackFastter){
            stateMachine.Animator.SetFloat("speed", 1.0f);
        }
        

        if(tryCombo)
        {
            tryCombo = GetRandomTryCombo();
            FacePlayer();
            attackChoosed = GetRandomMinotaurAttackCombo(attackChoosed);
            stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(attackChoosed), TransitionDuration));
        }else
        {
            stateMachine.SwitchState(new MinotaurChasingState(stateMachine));
        }
        
    }

    private IEnumerator WaitForSuperComboEnd(float transitionDuration)
    {   
        stateMachine.WeaponAxeDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.RightFootDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.LeftFootDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        attackChoosed = "attack1";
        int attackHash = Animator.StringToHash(attackChoosed);
        getTimeToWaitEndAnimation();
        
        stateMachine.Animator.SetFloat("speed", 1.5f);
        timeToWaitEndAnimation = (timeToWaitEndAnimation / 1.5f) - 0.2f;
        
        stateMachine.Animator.CrossFadeInFixedTime(attackHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        
        if(isInAttackRange()){
            attackChoosed = "attack2";
            attackHash = Animator.StringToHash(attackChoosed);
            getTimeToWaitEndAnimation();
        
            stateMachine.Animator.SetFloat("speed", 1.5f);
            timeToWaitEndAnimation = (timeToWaitEndAnimation / 1.5f) - 0.2f;

            stateMachine.Animator.CrossFadeInFixedTime(attackHash, transitionDuration);
            yield return new WaitForSeconds(timeToWaitEndAnimation);
        }
        
        
        if(isInAttackRange()){
            attackChoosed = "attack3";
            attackHash = Animator.StringToHash(attackChoosed);
            getTimeToWaitEndAnimation();
        
            stateMachine.Animator.SetFloat("speed", 1.5f);
            timeToWaitEndAnimation = (timeToWaitEndAnimation / 1.5f) - 0.2f;

            stateMachine.Animator.CrossFadeInFixedTime(attackHash, transitionDuration);
            yield return new WaitForSeconds(timeToWaitEndAnimation);
        }


        if(isInAttackRange()){
            attackChoosed = "attack4_kick";
            attackHash = Animator.StringToHash(attackChoosed);
            getTimeToWaitEndAnimation();
        
            stateMachine.Animator.SetFloat("speed", 1.5f);
            timeToWaitEndAnimation = (timeToWaitEndAnimation / 1.5f) - 0.2f;
            
            stateMachine.Animator.CrossFadeInFixedTime(attackHash, transitionDuration);
            yield return new WaitForSeconds(timeToWaitEndAnimation);
        }
        
        if(isInAttackRange()){
            attackChoosed = "attack5_kick";
            attackHash = Animator.StringToHash(attackChoosed);
            getTimeToWaitEndAnimation();
        
            stateMachine.Animator.SetFloat("speed", 1.5f);
            timeToWaitEndAnimation = (timeToWaitEndAnimation / 1.5f) - 0.2f;
            
            stateMachine.Animator.CrossFadeInFixedTime(attackHash, transitionDuration);
            yield return new WaitForSeconds(timeToWaitEndAnimation);
        }

        stateMachine.Animator.SetFloat("speed", 1.0f);
        stateMachine.SwitchState(new MinotaurChasingState(stateMachine));
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

    private string GetRandomMinotaurAttack()
    {
        stateMachine.WeaponAxeDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        int num = Random.Range(0,26);
        if(num <= 5 ){
            return "attack1";

        }else if(num <= 10){
            return "attack2";

        }else if(num <= 15){
            return "attack3";
        }else if(num <= 20){
            stateMachine.RightFootDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            return "attack4_kick";
        }
        stateMachine.LeftFootDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
       return "attack5_kick";
    }

    private string GetRandomMinotaurAttackCombo(string firstAttack)
    {
        stateMachine.WeaponAxeDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.RightFootDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.LeftFootDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);

        int num = Random.Range(0,21);
        if(firstAttack == "attack1")
        {
            if(num <= 5 ){
                return "attack2";
            }
            
            if(num <= 10 ){
                return "attack3";
            }

            if(num <= 15 ){
                return "attack4_kick";
            }

            return "attack5_kick";
            
        }

        if(firstAttack == "attack2")
        {
            if(num <= 5 ){
                return "attack1";
            }
            
            if(num <= 10 ){
                return "attack3";
            }
            
            if(num <= 15 ){
                return "attack4_kick";
            }

            return "attack5_kick";
            
        }

        if(firstAttack == "attack3")
        {
            if(num <= 5 ){
                return "attack1";
            }
            
            if(num <= 10 ){
                return "attack2";
            }
            
            if(num <= 15 ){
                return "attack4_kick";
            }

            return "attack5_kick";
            
        }

         if(firstAttack == "attack4_kick")
        {
            if(num <= 5 ){
                return "attack1";
            }
            
            if(num <= 10 ){
                return "attack2";
            }
            
            if(num <= 15 ){
                return "attack3";
            }

            return "attack5_kick";
            
        }

        
        if(num <= 5 ){
            return "attack1";
        }
        
        if(num <= 10 ){
            return "attack2";
        }

        if(num <= 15 ){
            return "attack3";
        }

        return "attack4_kick";

    }
    private bool isInAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }

    private void getTimeToWaitEndAnimation(){
        if(attackChoosed == "attack1"){
            timeToWaitEndAnimation = 1.55f;
        }else if(attackChoosed == "attack2"){
            timeToWaitEndAnimation = 1.8f;
        }else if(attackChoosed == "attack3"){
            timeToWaitEndAnimation = 2f;
        }else if(attackChoosed == "attack4_kick"){
            timeToWaitEndAnimation = 2.2f;
        }else if(attackChoosed == "attack5_kick"){
            timeToWaitEndAnimation = 1.6f;
        }
    }
}
