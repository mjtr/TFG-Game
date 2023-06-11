using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniDinoContinueEating : MiniDinoBaseState
{
    private const float TransitionDuration = 0.1f;

    private string attackChoosed;
    public MiniDinoContinueEating(MiniDinoStateMachine stateMachine) : base(stateMachine)    {   }

    public override void Enter()
    {   
        float newSpeedValue = Random.Range(0.3f,1.3f);
        stateMachine.Animator.SetFloat("Speed", newSpeedValue);
        int continueEatingHash = Animator.StringToHash("Eat_loop");
        stateMachine.Animator.CrossFadeInFixedTime(continueEatingHash, TransitionDuration);
    }

    public override void Tick(float deltaTime){ }

    public override void Exit(){ 
        stateMachine.isDinoEating = false;
    }

}
