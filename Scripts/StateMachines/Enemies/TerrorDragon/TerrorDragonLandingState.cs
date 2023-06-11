using System.Collections;
using UnityEngine;

public class TerrorDragonLandingState : TerrorDragonBaseState
{

    private string landAnimation = "Land";
    private const float CrossFadeDuration = 0.1f;
    public TerrorDragonLandingState(TerrorDragonStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {
        int landUpHash = Animator.StringToHash(landAnimation);
        stateMachine.StartCoroutine(WaitForAnimationToEnd(landUpHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(3.7f);
        stateMachine.SwitchState(new TerrorDragonChasingState(stateMachine));
        
    }
    public override void Tick(float deltaTime)
    { }

    public override void Exit(){
        stateMachine.SetIsFlying(false);
        stateMachine.ResetNavhMesh();
    }
}
