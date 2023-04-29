using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{

    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    private const float CrossFadeDuration = 0.1f;
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine){}

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

        if(stateMachine.InputReader.IsAttacking && stateMachine.CanStaminaPermitAttack(0)){
           stateMachine.SwitchState(new PlayerAttackingState(stateMachine,0));
           return;
        }

        if(stateMachine.InputReader.IsBlocking){
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }
        
        if(stateMachine.Targeter.CurrentTarget == null){
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement(deltaTime);
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);
       
        UpdateAnimator(deltaTime);
        
        stateMachine.Stamina.RecoverStamina();

        FaceTarget();
    }

    private void OnTarget()
    {
        stateMachine.Targeter.Cancel();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private void OnDodge()
    {
        if(stateMachine.InputReader.MovementValue == Vector2.zero){return;}
        if(!stateMachine.CanStaminaPermitDodge()){ return;}
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine,stateMachine.InputReader.MovementValue));
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

    private void OnRoll()
    {
        if(stateMachine.InputReader.MovementValue == Vector2.zero){return;}
        stateMachine.SwitchState(new PlayerRollingState(stateMachine,stateMachine.InputReader.MovementValue));    
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
