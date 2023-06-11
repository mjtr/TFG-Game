using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEaterDragonFlyingFireBreathState : SoulEaterDragonBaseState
{
    private const float TransitionDuration = 0.1f;
    private string attackChoosed = "Fly Fireball Shoot";
    private float timeToWaitEndAnimation = 1.47f;
    public SoulEaterDragonFlyingFireBreathState(SoulEaterDragonStateMachine stateMachine) : base(stateMachine)
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
        stateMachine.SwitchState(new SoulEaterDragonFlyingChasingState(stateMachine));
    }

    public override void Tick(float deltaTime){ 
        stateMachine.AddTimeToLandTime(deltaTime);
    }

    public override void Exit(){ }

  

}
