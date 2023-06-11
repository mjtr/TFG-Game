using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrorDragonFireBreathState : TerrorDragonBaseState
{
    private const float TransitionDuration = 0.1f;
    private string attackChoosed = "Flame Attack";
    private float timeToWaitEndAnimation = 4.4f;
    public TerrorDragonFireBreathState(TerrorDragonStateMachine stateMachine) : base(stateMachine)
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
        stateMachine.SwitchState(new TerrorDragonChasingState(stateMachine));
    }

    public override void Tick(float deltaTime){
        stateMachine.AddTimeToScreamTime(deltaTime);      
        stateMachine.AddTimeToFlyTime(deltaTime);
     }

    public override void Exit(){ }

  

}
