using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMonsterAttackingState : RockMonsterBaseState
{
    private const float TransitionDuration = 0.1f;

    private string attackChoosed;

    private float timeToWaitEndAnimation;
    public RockMonsterAttackingState(RockMonsterStateMachine stateMachine) : base(stateMachine)    {   }
    private bool tryCombo = false;
    private int countCombo = 0;

    public override void Enter()
    {   
        stateMachine.StopAllCourritines();
        stateMachine.StopSounds();
        stateMachine.DesactiveAllRockMonsterWeapon();
        attackChoosed = GetRandomRockMonsterAttack();
       
        int AttackHash = Animator.StringToHash(attackChoosed);
        FacePlayer();
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        tryCombo = GetRandomTryCombo();
        GetTimeToWaitEndAnimation();
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        if(tryCombo)
        {
            stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(GetRandomRockMonsterAttackCombo(attackChoosed)), TransitionDuration));
        }else
        {
            stateMachine.SwitchState(new RockMonsterIdleState(stateMachine));
        }
        
    }

    private void GetTimeToWaitEndAnimation()
    {
        if(attackChoosed == "Attack01a" || attackChoosed == "Attack01b")
        {
            timeToWaitEndAnimation = 2f;
            return;
        }
        
        if(attackChoosed == "Attack02")
        {
            timeToWaitEndAnimation = 2.43f;
            return;
        }

        if(attackChoosed == "Magic")
        {
            timeToWaitEndAnimation = 4f;
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

        if(attackChoosed == "Attack02" || attackChoosed == "Magic")
        {
            return false;
        }

        int num = Random.Range(0,20);
        if(num <= 8 ){
            return true;
        }
        return false;
    }

    public override void Tick(float deltaTime){ }

    public override void Exit()
    {
        stateMachine.ResetNavMesh();
        stateMachine.StopAllCourritines();
        stateMachine.StopSounds();
        stateMachine.DesactiveAllRockMonsterWeapon();
    }

    private string GetRandomRockMonsterAttack()
    {
        int num = Random.Range(0,20);
        if(num <= 5 ){
            stateMachine.ArmRightDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            return "Attack01a";

        }else if(num <= 10){
            stateMachine.ArmLeftDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            return "Attack01b";

        }else if(num <= 15){
            stateMachine.ArmRightDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            stateMachine.ArmLeftDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
            return "Attack02";
        }
       stateMachine.RockMonsterLasser.LaserWeaponLogic.GetComponent<WeaponDamage>().SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);       
       return "Magic";
    }

    private string GetRandomRockMonsterAttackCombo(string firstAttack)
    {
        int num = Random.Range(0,20);
        if(firstAttack == "Attack01a")
        {
            if(num <= 10 ){
                stateMachine.ArmLeftDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
                return "Attack01b";
            }
        }

        if(firstAttack == "Attack01b")
        {
            if(num <= 10 ){
                stateMachine.ArmRightDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
                return "Attack01a";
            }
        }
        stateMachine.ArmRightDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.ArmLeftDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        return "Attack02";
    }
    private bool isInAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }
}
