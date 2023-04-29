using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

public class WarriorPlayerShiftAttackingState : WarriorPlayerBaseState
{
    private float previousFrameTime;
    private bool alreadyApplyForce; 
    private ShiftAttack attack;

    public WarriorPlayerShiftAttackingState(WarriorPlayerStateMachine stateMachine, int shiftAttackIndex) : base(stateMachine){
       attack = stateMachine.ShiftAttacks[shiftAttackIndex];
    }

    public override void Enter()
    {
        stateMachine.isAttacking = true;
        stateMachine.Stamina.TakeStamina(attack.staminaTaked);
        stateMachine.GetWeaponDamage().SetAttack(stateMachine.GetDamageStat() + attack.ExtraDamage, attack.Knockback);
        if(stateMachine.getIsTwoHandsWeapon()){
            stateMachine.Animator.SetFloat("Speed", 0.65f);
        }else{
            stateMachine.Animator.SetFloat("Speed", 1f);
        }
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName,attack.TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        FaceTarget();
       
        float normalizedTime = GetNormalizeTime(stateMachine.Animator,"Attack");
        
        if(normalizedTime >= previousFrameTime && normalizedTime < 1f){
            
            if(normalizedTime >= attack.ForceTime)
            {
                TryApplyForce();
            }
            
            TryComboAttack(normalizedTime);
            
        }else{
            //una vez terminado el combo, volveremos o al estado de tener fijado al enemigo o al estado libre
            if(stateMachine.Targeter.CurrentTarget != null){
                stateMachine.SwitchState(new WarriorPlayerTargetingState(stateMachine));
            }else{
                stateMachine.SwitchState(new WarriorPlayerFreeLookState(stateMachine));
            }
        }
        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
        stateMachine.isAttacking = false;
    }

    private void TryComboAttack(float normalizedTime)
    {
        if(attack.ComboStateIndex == -1){ return;}
        if(normalizedTime < attack.ComboAttackTime){return; }
        
        if(stateMachine.IsRighMouseClickButtonMainteinPressed()){
            if(!stateMachine.CanStaminaPermitShiftAttack(attack.ComboStateIndex)){return;}
            stateMachine.SwitchState(new WarriorPlayerShiftAttackingState(stateMachine,attack.ComboStateIndex));
            return;
        }

        if(!stateMachine.InputReader.IsAttacking || !stateMachine.CanStaminaPermitAttack(attack.ComboStateIndex)){return;}
        stateMachine.SwitchState(new WarriorPlayerAttackingState(stateMachine,attack.ComboStateIndex));
    }

    private void TryApplyForce()
    {
        if(alreadyApplyForce){ return; }
        stateMachine.ForceReceived.AddForce(stateMachine.transform.forward * attack.Force);
        alreadyApplyForce = true;
    }

}
