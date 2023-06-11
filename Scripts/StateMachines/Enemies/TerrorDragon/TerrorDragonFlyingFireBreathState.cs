using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrorDragonFlyingFireBreathState : TerrorDragonBaseState
{
    private const float TransitionDuration = 0.1f;
    private string attackChoosed = "Fly Flame Attack";
    private float timeToWaitEndAnimation = 3.3f;
    public TerrorDragonFlyingFireBreathState(TerrorDragonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {   
        int AttackHash = Animator.StringToHash(attackChoosed);
        FacePlayer();
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new TerrorDragonFlyingChasingState(stateMachine));
    }

    public override void Tick(float deltaTime){ 
        stateMachine.AddTimeToLandTime(deltaTime);
    }

    public override void Exit(){ }

  

}
