using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurImpactState : MinotaurBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("hit_1");
    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 1f;

    public MinotaurImpactState(MinotaurStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.StartCoroutine(WaitForAnimationToEnd(ImpactHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new MinotaurChasingState(stateMachine));
    }


    public override void Tick(float deltaTime){ }

    public override void Exit(){
        stateMachine.ResetNavMesh();
    }
}
