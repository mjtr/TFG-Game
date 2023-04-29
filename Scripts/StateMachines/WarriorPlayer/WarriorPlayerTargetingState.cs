using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorPlayerTargetingState : WarriorPlayerBaseState
{

    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");

    private const float CrossFadeDuration = 0.1f;
    public WarriorPlayerTargetingState(WarriorPlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        stateMachine.StopParticlesEffects();
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;

        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.IsRighMouseClickButtonMainteinPressed()){
            if(!stateMachine.CanStaminaPermitShiftAttack(0)){return; }
            stateMachine.SwitchState(new WarriorPlayerShiftAttackingState(stateMachine, 0));
            return;
        }

        if(stateMachine.InputReader.IsAttacking && stateMachine.CanStaminaPermitAttack(0)){
           stateMachine.SwitchState(new WarriorPlayerAttackingState(stateMachine,0));
           return;
        }

        if(stateMachine.InputReader.IsBlocking){
            stateMachine.SwitchState(new WarriorPlayerBlockingState(stateMachine));
            return;
        }
        
        if(stateMachine.Targeter.CurrentTarget == null){
            stateMachine.SwitchState(new WarriorPlayerFreeLookState(stateMachine));
            return;
        }

        if(stateMachine.IsSpaceButtonPressedAndHasStamina()){
            stateMachine.SwitchState(new WarriorPlayerSpecialAttackState(stateMachine));
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
        stateMachine.SwitchState(new WarriorPlayerFreeLookState(stateMachine));
    }

    private void OnDodge()
    {
        if(stateMachine.InputReader.MovementValue == Vector2.zero){return;}
        if(!stateMachine.CanStaminaPermitDodge()){ return;}
        stateMachine.SwitchState(new WarriorPlayerDodgingState(stateMachine,stateMachine.InputReader.MovementValue));
    }

    private void OnJump()
    {
        //stateMachine.SwitchState(new WarriorPlayerJumpingState(stateMachine));
    }

    private Vector3 CalculateMovement (float deltaTime){
        Vector3 movement = new Vector3();

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
