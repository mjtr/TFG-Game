using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerFallingState : HumanPlayerBaseState
{
    private readonly int FallHash = Animator.StringToHash("Fall");

    private Vector3 momentum;
    private const float CrossFadeDuration = 0.1f;

    public HumanPlayerFallingState(HumanPlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        momentum = stateMachine.Controller.velocity;
        momentum.y = 0;
        stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }

    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum,deltaTime);

        if(stateMachine.Controller.isGrounded){
            ReturnToLocomotion();
        }

        FaceTarget();
    }

    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        stateMachine.SwitchState(new HumanPlayerHangingState(stateMachine,ledgeForward,closestPoint));
    }
}