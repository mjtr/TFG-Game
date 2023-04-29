using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoIdleState : RhinoBaseState
{

     private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");

    private readonly int SpeedHash = Animator.StringToHash("locomotion");

    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;


    public RhinoIdleState(RhinoStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllRhinoWeapon();
        stateMachine.Animator.SetFloat(SpeedHash, 0f);
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {

        Move(deltaTime);

        if(!IsInChaseRange())
        {
            stateMachine.isDetectedPlayed = false;
            if(stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new RhinoPatrolPathState(stateMachine));
                return;
            }
            return;
        }

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            if(stateMachine.GetFirsTimeTotSeePlayer())
            {
                stateMachine.isDetectedPlayed = true;
                stateMachine.SwitchState(new RhinoScreamState(stateMachine));
                return;
            }

            stateMachine.isDetectedPlayed = true;
            stateMachine.SwitchState(new RhinoChasingState(stateMachine));
            return;
        }
        
        stateMachine.isDetectedPlayed = false;        
    }

    public override void Exit(){ }


}
