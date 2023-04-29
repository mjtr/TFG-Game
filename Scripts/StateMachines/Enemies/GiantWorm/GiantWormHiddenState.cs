using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantWormHiddenState : GiantWormBaseState
{
    private readonly int GiantWormHiddenHash = Animator.StringToHash("Hidden");

    private const float CrossFadeDuration = 0.1f;
    public GiantWormHiddenState(GiantWormStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.DesactiveAllWormWeapon();
        stateMachine.StopAllCoroutines();
        stateMachine.Animator.CrossFadeInFixedTime(GiantWormHiddenHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {

        if(IsInChaseRange())
        {
            FacePlayer();
            stateMachine.SwitchState(new GiantWormAppearState(stateMachine));
            return;
        }

    }

    public override void Exit(){ }
}
