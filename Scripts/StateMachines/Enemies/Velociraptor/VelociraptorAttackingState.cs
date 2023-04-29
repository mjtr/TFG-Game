using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelociraptorAttackingState : VelociraptorBaseState
{
    private const float TransitionDuration = 0.1f;
    private string attackChoosed;
    private bool tryCombo = false;
    private float timeToWaitEndAnimation;
    private int countCombo = 0;

    public VelociraptorAttackingState(VelociraptorStateMachine stateMachine) : base(stateMachine)    {   }

    public override void Enter()
    {   
        stateMachine.StopAllCourritines();
        stateMachine.DesactiveAllVelociraptorWeapon();
        attackChoosed = GetRandomVelociraptorAttack();
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
            stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(GetRandomVelociraptorAttackCombo(attackChoosed)), TransitionDuration));
        }else
        {
            stateMachine.SwitchState(new VelociraptorIdleState(stateMachine));
        }
        
    }

    private void GetTimeToWaitAnimation()
    {
         stateMachine.HeadDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        if(attackChoosed == "Bite1"){
            timeToWaitEndAnimation = 2.4f;
            return;
        }
        
        timeToWaitEndAnimation = 2f;
        return;
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

    public override void Tick(float deltaTime){ }

    public override void Exit(){ 
        stateMachine.ResetNavhMesh();
    }

    private string GetRandomVelociraptorAttack()
    {
        stateMachine.EnableArmsDamage();
        int num = Random.Range(0,10);
        if(num <= 5 ){
            return "Bite1";

        }
        return "Bite2";
    }

    private string GetRandomVelociraptorAttackCombo(string firstAttack)
    {
        stateMachine.EnableArmsDamage();
        int num = Random.Range(0,10);
        if(firstAttack == "Bite1")
        {
            if(num <= 5 ){
                return "Bite2";
            }
            return "Bite1";
        }
        
        if(num <= 5 ){
            return "Bite1";
        }
        
        return "Bite2";

    }
    private bool isInAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
