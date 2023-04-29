using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorPlayerBlockingState : WarriorPlayerBaseState
{
    private readonly int PlayerBlockingHash = Animator.StringToHash("Block");

    private const float CrossFadeDuration = 0.1f;
    public WarriorPlayerBlockingState(WarriorPlayerStateMachine stateMachine) : base(stateMachine){ }

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
            if(stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new WarriorPlayerTargetingState(stateMachine));
                return;
            }else{
                stateMachine.SwitchState(new WarriorPlayerFreeLookState(stateMachine));
                return;
            }
        }
       
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }
}
