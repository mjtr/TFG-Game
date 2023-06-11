using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantWormImpactState : GiantWormBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("GotHitBody");

    private const float CrossFadeDuration = 0.1f;

    private float timeToWaitEndAnimation = 2.17f;

    public GiantWormImpactState(GiantWormStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.StopAllCoroutines();
        stateMachine.DesactiveAllWormWeapon();
        stateMachine.StartCoroutine(WaitForAnimationToEnd(ImpactHash, CrossFadeDuration));        
    }
    
    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new GiantWormIdleState(stateMachine));
    }

    public override void Tick(float deltaTime)
    {}

    public override void Exit(){ }
}
