using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerFreeLookState : HumanPlayerBaseState
{
    private bool shouldFade;
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");

    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");

    private const float AnimatorDampTime = 0.1f;

    private const float CrossFadeDuration = 0.1f;
    
    //El booleano es para cuando escalamos, para hacer ajustes en los cambios de animación
    public HumanPlayerFreeLookState(HumanPlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine)
    {
        this.shouldFade = shouldFade;
    }
    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += onTarget;
        stateMachine.InputReader.RollEvent += OnRoll;

        
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0f);
        if(shouldFade)
        {
            stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
        }else{
            stateMachine.Animator.Play(FreeLookBlendTreeHash);
        }

    }

    public override void Tick(float deltaTime){

        if(stateMachine.InputReader.IsAttacking){
           stateMachine.SwitchState(new HumanPlayerAttackingState(stateMachine, 0));
           return;
        }

        Vector3 movement = CalculateMovement();
       
        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);
        
        if(stateMachine.InputReader.MovementValue == Vector2.zero){ 
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }
    
    public override void Exit(){
        stateMachine.InputReader.TargetEvent -= onTarget;
        stateMachine.InputReader.RollEvent -= OnRoll;
    }

    private void onTarget(){
        if(!stateMachine.Targeter.SelectTarget()){ return ;}
        stateMachine.SwitchState(new HumanPlayerTargetingState(stateMachine));
    } 


    private void OnJump()
    {
        stateMachine.SwitchState(new HumanPlayerJumpingState(stateMachine));
    }

    private void OnRoll()
    {
        if(stateMachine.InputReader.MovementValue == Vector2.zero){return;}
        stateMachine.SwitchState(new HumanPlayerRollingState(stateMachine,stateMachine.InputReader.MovementValue));
        return;
    }


       
    private void FaceMovementDirection(Vector3 movement, float deltaTime){
        stateMachine.transform.rotation = Quaternion.Lerp(
            stateMachine.transform.rotation,
            Quaternion.LookRotation(movement),
            deltaTime * stateMachine.RotationDamping);
    }

    private Vector3 CalculateMovement(){
        Vector3 forward =stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x ;
    }

}
