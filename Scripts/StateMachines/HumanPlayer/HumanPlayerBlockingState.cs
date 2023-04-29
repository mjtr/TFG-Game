using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerBlockingState : HumanPlayerBaseState
{
    private readonly int PlayerBlockingHash = Animator.StringToHash("Block");

    private const float CrossFadeDuration = 0.1f;
    public HumanPlayerBlockingState(HumanPlayerStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.Health.SetInvulnerable(true);
        stateMachine.Animator.CrossFadeInFixedTime(PlayerBlockingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        if(!stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new HumanPlayerTargetingState(stateMachine));
            return;
        }

        if(stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new HumanPlayerFreeLookState(stateMachine));
            return;
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }
}
