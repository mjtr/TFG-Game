using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantWormDisappearState : GiantWormBaseState
{
    private readonly int GiantWormDisappearHash = Animator.StringToHash("Disappear");

    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 2.08f;
    public GiantWormDisappearState(GiantWormStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
           stateMachine.StartCoroutine(WaitForAnimationToEnd(GiantWormDisappearHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new GiantWormHiddenState(stateMachine));
    }

    public override void Tick(float deltaTime)
    {}

    public override void Exit(){ 
        if(stateMachine.GetIsActionMusicStart())
        {
            stateMachine.StartAmbientMusic();
        }
    }
}
