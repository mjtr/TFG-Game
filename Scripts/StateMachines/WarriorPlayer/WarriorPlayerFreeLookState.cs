using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorPlayerFreeLookState : WarriorPlayerBaseState
{
    private bool shouldFade;
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;
    private float sprintValue = 0f;
    
    //El booleano es para cuando escalamos, para hacer ajustes en los cambios de animación
    public WarriorPlayerFreeLookState(WarriorPlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine)
    {
        this.shouldFade = shouldFade;
    }
    public override void Enter()
    {
        stateMachine.StopParticlesEffects();
        stateMachine.InputReader.TargetEvent += onTarget;
        stateMachine.GetWeaponDamage().gameObject.SetActive(false);
        
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0f);
        if(shouldFade)
        {
            stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
        }else{
            stateMachine.Animator.Play(FreeLookBlendTreeHash);
        }

    }

    public override void Tick(float deltaTime){
        if (GUIUtility.hotControl != 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        if(stateMachine.InputReader.IsBlocking){
            stateMachine.SwitchState(new WarriorPlayerBlockingState(stateMachine));
            return;
        }

        if(stateMachine.IsRighMouseClickButtonMainteinPressed() && stateMachine.CanStaminaPermitShiftAttack(0)){
            stateMachine.SwitchState(new WarriorPlayerShiftAttackingState(stateMachine, 0));
            return;
        }

        if(stateMachine.InputReader.IsAttacking && stateMachine.CanStaminaPermitAttack(0)){
           stateMachine.SwitchState(new WarriorPlayerAttackingState(stateMachine, 0));
           return;
        }

        if(stateMachine.IsSpaceButtonPressedAndHasStamina()){
            stateMachine.SwitchState(new WarriorPlayerSpecialAttackState(stateMachine));
            return;
        }

        bool isSprintMovement = IsShiftButtonPressedAndHasStamina();
        Vector3 movement = CalculateMovement();
       
        Move(movement * (stateMachine.FreeLookMovementSpeed + sprintValue), deltaTime);     

        if(stateMachine.InputReader.MovementValue == Vector2.zero){ 
            stateMachine.Stamina.RecoverStamina();
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            return;
        }
        
        if(isSprintMovement){
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1.5f, AnimatorDampTime, deltaTime);
            FaceMovementDirection(movement, deltaTime);
            stateMachine.Stamina.TakeStamina(stateMachine.SprintStaminaTaked);
            return;
        }

        stateMachine.Stamina.RecoverStamina();
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1f, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }
    
    public override void Exit(){
        stateMachine.InputReader.TargetEvent -= onTarget;
    }

    private void onTarget(){
        if(!stateMachine.Targeter.SelectTarget()){ return ;}
        stateMachine.SwitchState(new WarriorPlayerTargetingState(stateMachine));
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

      private bool IsShiftButtonPressedAndHasStamina()
    {
        
        if(!stateMachine.Stamina.CanStaminaPermitAction(stateMachine.SprintStaminaTaked)){
            sprintValue = 0f;
            return false;
        }

        if(!stateMachine.IsShiftButtonMainteinPressed()){
            sprintValue = 0f;
            return false;
        }

        sprintValue = 2.0f;
        return true;
    }

}
