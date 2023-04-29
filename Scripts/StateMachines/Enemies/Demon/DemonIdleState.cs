using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonIdleState : DemonBaseState
{
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    public DemonIdleState(DemonStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDemonWeapon();
        stateMachine.Animator.SetFloat(LocomotionHash, 0f);
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        
        Move(deltaTime);

        if(!IsInChaseRange())
        {
            //Vamos a hacer aquí que el dragon patrulle
            stateMachine.isDetectedPlayed = false;
            if(stateMachine.PatrolPath != null)
            {
                stateMachine.SwitchState(new DemonPatrolPathState(stateMachine));
                return;
            }
            return;
        }

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            stateMachine.isDetectedPlayed = true;
            stateMachine.SwitchState(new DemonChasingState(stateMachine));
            return;
        }
        
        stateMachine.isDetectedPlayed = false;

        //FacePlayer();
       // stateMachine.Animator.SetFloat(SpeedHash,0f, AnimatorDampTime, deltaTime);
    }

    public override void Exit(){ }


}
