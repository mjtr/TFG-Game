using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonUsurperClawAttackState : DragonUsurperBaseState
{
   private const float TransitionDuration = 0.1f;

    private string attackChoosed = "Claw Attack";
    private float timeToWaitEndAnimation = 3f;
    public DragonUsurperClawAttackState(DragonUsurperStateMachine stateMachine) : base(stateMachine)    {   }

    public override void Enter()
    {   
        stateMachine.StopAllCourritines();
        stateMachine.DesactiveAllDragonUsurperWeapon();
        int AttackHash = Animator.StringToHash(attackChoosed);
        FacePlayer();
        stateMachine.HeadDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.ArmLeftDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.ArmRightDamage.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new DragonUsurperChasingState(stateMachine));
        
    }
    public override void Tick(float deltaTime){ }

    public override void Exit(){ }
}
