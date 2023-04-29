using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private float previousFrameTime;
    private bool alreadyApplyForce; 
    private Attack attack;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine){
       attack = stateMachine.Attacks[attackIndex];
    }

    public override void Enter()
    {
        Debug.Log(attack.AnimationName);
        stateMachine.Stamina.TakeStamina(attack.staminaTaked);
        stateMachine.Weapon.SetAttack(stateMachine.GetDamageStat() + attack.ExtraDamage, attack.Knockback);
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
            
            if(stateMachine.InputReader.IsAttacking){
                TryComboAttack(normalizedTime);
            }
        }else{
            //una vez terminado el combo, volveremos o al estado de tener fijado al enemigo o al estado libre
            if(stateMachine.Targeter.CurrentTarget != null){
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }else{
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }
        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {

    }

    private void TryComboAttack(float normalizedTime)
    {
        if(attack.ComboStateIndex == -1){ return;}
        if(normalizedTime < attack.ComboAttackTime){return; }
        if(!stateMachine.CanStaminaPermitAttack(attack.ComboStateIndex)){return;}
        
        stateMachine.SwitchState(new PlayerAttackingState(stateMachine,attack.ComboStateIndex));
    }

    private void TryApplyForce()
    {
        if(alreadyApplyForce){ return; }
        stateMachine.ForceReceived.AddForce(stateMachine.transform.forward * attack.Force);
        alreadyApplyForce = true;
    }

}
