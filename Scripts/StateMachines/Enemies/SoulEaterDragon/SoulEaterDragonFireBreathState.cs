using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEaterDragonFireBreathState : SoulEaterDragonBaseState
{
    private const float TransitionDuration = 0.1f;
    private string attackChoosed = "Fireball Shoot";
    private float timeToWaitEndAnimation = 2.2f;
    public SoulEaterDragonFireBreathState(SoulEaterDragonStateMachine stateMachine) : base(stateMachine)
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
        stateMachine.SwitchState(new SoulEaterDragonChasingState(stateMachine));
    }

    public override void Tick(float deltaTime){
        stateMachine.AddTimeToFlyTime(deltaTime);
     }

    public override void Exit(){ }

  

}
