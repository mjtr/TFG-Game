using System.Collections;
using UnityEngine;

public class SpiderDodgeState : SpiderBaseState
{

    private readonly int BlockHash = Animator.StringToHash("Dodge");

    private const float CrossFadeDuration = 0.1f;

    private float timeToWaitEndAnimation = 1.25f;

    public SpiderDodgeState(SpiderStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        FacePlayer();
        stateMachine.DesactiveAllSpiderWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.Health.SetInvulnerable(true);
        stateMachine.StartCoroutine(WaitForAnimationToEnd(BlockHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new SpiderIdleState(stateMachine));
    }


    public override void Tick(float deltaTime) {  }

    public override void Exit(){ 
        stateMachine.Health.SetInvulnerable(false);
    }
}
