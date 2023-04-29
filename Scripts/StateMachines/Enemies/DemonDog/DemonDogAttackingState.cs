using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonDogAttackingState : DemonDogBaseState
{
    private readonly int AttackHash = Animator.StringToHash("BiteAttack1");
    private const float TransitionDuration = 0.1f;

    private float timeToWaitEndAnimation = 1.2f;

    public DemonDogAttackingState(DemonDogStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {   
        stateMachine.isDetectedPlayed = true;
        FacePlayer();
        stateMachine.Weapon.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new DemonDogIdleState(stateMachine));
    }

    public override void Tick(float deltaTime){  }

    public override void Exit(){
        stateMachine.ResetNavMesh();
    }

}
