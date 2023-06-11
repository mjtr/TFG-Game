using System.Collections;
using UnityEngine;

public class SoulEaterDragonStartFlyingState : SoulEaterDragonBaseState
{

    private string takeOfAnimation = "Take Off";
    private const float CrossFadeDuration = 0.1f;
    public SoulEaterDragonStartFlyingState(SoulEaterDragonStateMachine stateMachine) : base(stateMachine)
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
        yield return new WaitForSeconds(2.5f);
        stateMachine.SwitchState(new SoulEaterDragonFlyingChasingState(stateMachine));
        
    }
    public override void Tick(float deltaTime)
    { }

    public override void Exit()
    { 
        
    }
}
