using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorPlayerJumpingState : WarriorPlayerBaseState
{

    private readonly int JumpHash = Animator.StringToHash("Jump");
    private Vector3 momentum;

    private const float CrossFadeDuration = 0.5f;

    public WarriorPlayerJumpingState(WarriorPlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        stateMachine.ForceReceived.Jump(stateMachine.JumpForce);

        momentum = stateMachine.Controller.velocity;
        momentum.y = 0f;

        stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);

        stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
    }


    public override void Tick(float deltaTime)
    {
        Move(momentum,deltaTime);

        if(stateMachine.Controller.velocity.y <= 0)
        {
            stateMachine.SwitchState(new WarriorPlayerFallingState(stateMachine));
            return;
        }

        FaceTarget();
    }
    
    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
    }

    private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
    {
        stateMachine.SwitchState(new WarriorPlayerHangingState(stateMachine,ledgeForward,closestPoint));
    }
}
