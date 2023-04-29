using System.Collections;
using UnityEngine;

public class DemonBlockState : DemonBaseState
{

    private readonly int BlockHash = Animator.StringToHash("Idle_Block");

    private const float CrossFadeDuration = 0.1f;

    private float timeToWaitEndAnimation = 1.5f;

    public DemonBlockState(DemonStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        FacePlayer();
        stateMachine.DesactiveAllDemonWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.Health.SetInvulnerable(true);
        stateMachine.StartCoroutine(WaitForAnimationToEnd(BlockHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new DemonIdleState(stateMachine));
    }


    public override void Tick(float deltaTime) {  }

    public override void Exit(){ 
        stateMachine.Health.SetInvulnerable(false);
    }
}
