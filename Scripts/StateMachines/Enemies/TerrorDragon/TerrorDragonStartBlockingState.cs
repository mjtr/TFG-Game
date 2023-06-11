using System.Collections;
using UnityEngine;

public class TerrorDragonStartBlockingState : TerrorDragonBaseState
{

    private readonly int BlockHash = Animator.StringToHash("Defend");
    private const float CrossFadeDuration = 0.1f;

    public TerrorDragonStartBlockingState(TerrorDragonStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.StartCoroutine(WaitForAnimationToEnd(BlockHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(1.7f);
        stateMachine.SwitchState(new TerrorDragonChasingState(stateMachine));
        
    }

    public override void Tick(float deltaTime)
    {
       
    }

    public override void Exit(){ 
        stateMachine.Health.SetInvulnerable(false);
    }
}
