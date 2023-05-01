using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerDeadState : HumanPlayerBaseState
{

    private readonly int PlayerDeadHash = Animator.StringToHash("PlayerDeath");

    private const float CrossFadeDuration = 0.1f;

    public HumanPlayerDeadState(HumanPlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        stateMachine.Ragdoll.ToggleRagdoll(true);
        stateMachine.Weapon.gameObject.SetActive(false);
        stateMachine.Animator.CrossFadeInFixedTime(PlayerDeadHash, CrossFadeDuration);
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {

    }
}
