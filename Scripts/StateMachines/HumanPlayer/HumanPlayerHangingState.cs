using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerHangingState : HumanPlayerBaseState
{
    private readonly int PlayerHangingHash = Animator.StringToHash("Hanging");

    private const float CrossFadeDuration = 0.1f;
    private Vector3 ledgeForward;
    private Vector3 closestPoint;

    public HumanPlayerHangingState(HumanPlayerStateMachine stateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(stateMachine)
    { 
        this.ledgeForward = ledgeForward;
        this.closestPoint = closestPoint;
    }

    public override void Enter()
    {
        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward,Vector3.up);

        stateMachine.Controller.enabled = false;
        stateMachine.transform.position = closestPoint - (stateMachine.LedgeDetector.transform.position - stateMachine.transform.position);//para arreglar los agarrer a los salientes
        stateMachine.Controller.enabled = true;

        stateMachine.Animator.CrossFadeInFixedTime(PlayerHangingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.InputReader.MovementValue.y > 0f)
        {
            stateMachine.SwitchState(new HumanPlayerPullUpState(stateMachine));
        }
        else if(stateMachine.InputReader.MovementValue.y < 0f)
        {
            stateMachine.Controller.Move(Vector3.zero);
            stateMachine.ForceReceived.Reset();
            stateMachine.SwitchState(new HumanPlayerFallingState(stateMachine));
        }
    }
    public override void Exit()
    {

    }


}
