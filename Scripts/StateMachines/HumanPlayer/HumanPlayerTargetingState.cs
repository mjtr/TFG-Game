using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayerTargetingState : HumanPlayerBaseState
{

    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    private const float CrossFadeDuration = 0.1f;
    public HumanPlayerTargetingState(HumanPlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.RollEvent += OnRoll;

        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.RollEvent -= OnRoll;
    }

    public override void Tick(float deltaTime)
    {

        if(stateMachine.InputReader.IsAttacking){
           stateMachine.SwitchState(new HumanPlayerAttackingState(stateMachine,0));
           return;
        }

        if(stateMachine.InputReader.IsBlocking){
            stateMachine.SwitchState(new HumanPlayerBlockingState(stateMachine));
            return;
        }
        
        if(stateMachine.Targeter.CurrentTarget == null){
            stateMachine.SwitchState(new HumanPlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement(deltaTime);
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);
       
        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    private void OnTarget()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new HumanPlayerFreeLookState(stateMachine));
    }

    private void OnDodge()
    {
        if(stateMachine.InputReader.MovementValue == Vector2.zero){return;}
        stateMachine.SwitchState(new HumanPlayerDodgingState(stateMachine,stateMachine.InputReader.MovementValue));
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new HumanPlayerJumpingState(stateMachine));
    }

    private void OnRoll()
    {
        if(stateMachine.InputReader.MovementValue == Vector2.zero){return;}
        stateMachine.SwitchState(new HumanPlayerRollingState(stateMachine,stateMachine.InputReader.MovementValue));    
    }

    private Vector3 CalculateMovement (float deltaTime){
        Vector3 movement = new Vector3();
    
           // remainingDodgeTime = Mathf.Max(remainingDodgeTime - deltaTime, 0f);
            //Lo comentado abajo es lo mismo que la linea de arriba
           // remainingDodgeTime -= deltaTime;
            //if(remainingDodgeTime < 0f){
            //    remainingDodgeTime = 0f;
            //}
      
        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x ;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;
        
        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        
        if(stateMachine.InputReader.MovementValue.y == 0){
            stateMachine.Animator.SetFloat(TargetingForwardHash,0, 0.1f, deltaTime);
        }else{
            float value = stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingForwardHash,value, 0.1f, deltaTime);
        }

        if(stateMachine.InputReader.MovementValue.x == 0){
            stateMachine.Animator.SetFloat(TargetingRightHash,0, 0.1f, deltaTime);
        }else{
            float value = stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingRightHash,value, 0.1f, deltaTime);
        }

    }

}
