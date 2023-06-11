using System.Collections;
using UnityEngine;

public class TerrorDragonStartFlyingState : TerrorDragonBaseState
{

    private string takeOfAnimation = "Take Off";
    private const float CrossFadeDuration = 0.1f;
    public TerrorDragonStartFlyingState(TerrorDragonStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {
        stateMachine.SetIsFlying(true);
        int takeOffHash = Animator.StringToHash(takeOfAnimation);
        stateMachine.StartCoroutine(WaitForAnimationToEnd(takeOffHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(2.8f);
        stateMachine.SwitchState(new TerrorDragonFlyingChasingState(stateMachine));
        
    }
    public override void Tick(float deltaTime)
    { }

    public override void Exit()
    { 
        
    }
}
