using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrollImpactState : TrollBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("Get_hit");

    private const float CrossFadeDuration = 0.1f;

    private float timeToWaitEndAnimation = 0.8f;

    public TrollImpactState(TrollStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.StartCoroutine(WaitForAnimationToEnd(ImpactHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new TrollIdleState(stateMachine));
    }

    public override void Tick(float deltaTime)
    {}

    public override void Exit(){
        stateMachine.ResetNavhMesh();
    }
}
