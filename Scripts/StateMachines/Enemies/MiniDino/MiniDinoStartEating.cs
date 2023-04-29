using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniDinoStartEatingState : MiniDinoBaseState
{
    private const float TransitionDuration = 0.1f;
    public MiniDinoStartEatingState(MiniDinoStateMachine stateMachine) : base(stateMachine)    {   }

    public override void Enter()
    {   
        int startEat = Animator.StringToHash("Eat_start");
        stateMachine.StartCoroutine(WaitForAnimationToEnd(startEat, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(stateMachine.Animator.GetCurrentAnimatorStateInfo(0).length + 0.5f);
        stateMachine.SwitchState(new MiniDinoContinueEating(stateMachine));
    }

    public override void Tick(float deltaTime){ }

    public override void Exit(){ }
}
