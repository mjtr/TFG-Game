using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollingState : PlayerBaseState
{
    
    private readonly int RollingHash = Animator.StringToHash("Roll");

    private readonly string RollTrigger = "RollTrigger";

    private Vector3 rollingDirectionInput;
    private float remainingDodgeTime;
    private const float CrossFadeDuration = 0.5f;
    
    public PlayerRollingState(PlayerStateMachine stateMachine, Vector3 rollingDirectionInput) : base(stateMachine)
    {
        this.rollingDirectionInput = rollingDirectionInput;
    }

    public override void Enter()
    {
        Debug.Log("Vamos a rodar");
        remainingDodgeTime = stateMachine.RollDuration;
        stateMachine.Animator.SetTrigger(RollTrigger);
        //stateMachine.Animator.CrossFadeInFixedTime(RollingHash, CrossFadeDuration);
        stateMachine.Health.SetInvulnerable(true);
    }
    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3(); 

        movement += stateMachine.transform.right * rollingDirectionInput.x * stateMachine.RollLength / stateMachine.RollDuration;
        movement += stateMachine.transform.forward * rollingDirectionInput.y * stateMachine.RollLength / stateMachine.RollDuration;
        
        Move(movement,deltaTime);

        remainingDodgeTime -= deltaTime;
        if(remainingDodgeTime <= 0f)
        {
            if(stateMachine.Targeter.CurrentTarget == null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
                return;
            }
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }
    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }

   
}
