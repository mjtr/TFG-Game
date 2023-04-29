using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoImpactState : RhinoBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("Get_Hit");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public RhinoImpactState(RhinoStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.DesactiveAllRhinoWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        duration -= deltaTime;
        if(duration <= 0f)
        {
            stateMachine.SwitchState(new RhinoIdleState(stateMachine));
        }
    }

    public override void Exit(){
        stateMachine.ResetNavMesh();
    }
}
