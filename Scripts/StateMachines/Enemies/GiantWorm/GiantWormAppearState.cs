using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantWormAppearState : GiantWormBaseState
{
    private readonly int GiantWormAppearHash = Animator.StringToHash("Appear");

    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 3.05f;

    private bool isFirstTime = true;

    public GiantWormAppearState(GiantWormStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.DesactiveAllWormWeapon();
        stateMachine.StopAllCoroutines();
        if(isFirstTime)
        {
            isFirstTime = false;
            stateMachine.SetChasingRange(stateMachine.PlayerChasingRange + 3f);
        }
        stateMachine.StartActionMusic();
        stateMachine.SetAudioControllerIsAttacking(true);
        
        stateMachine.StartCoroutine(WaitForAnimationToEnd(GiantWormAppearHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new GiantWormIdleState(stateMachine));
    }

    public override void Tick(float deltaTime){
    }

    public override void Exit(){ 
    }
}
