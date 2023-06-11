using System.Collections;
using UnityEngine;

public class SpiderScreamState : SpiderBaseState
{
    private string screamAnimation = "Taunt";
    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 5.87f;
    public SpiderScreamState(SpiderStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {
        FacePlayer();
        stateMachine.DesactiveAllSpiderWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(screamAnimation), CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new SpiderChasingState(stateMachine));
    }
     
    public override void Tick(float deltaTime)
    { }

    public override void Exit(){ 
    }
}
