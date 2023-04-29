using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorPlayerPickupState : WarriorPlayerBaseState
{
    private readonly int PlayerPickupgHash = Animator.StringToHash("PickingUp");

    private const float CrossFadeDuration = 0.5f;
    private bool isTwoHandsWeapon;

    public WarriorPlayerPickupState(WarriorPlayerStateMachine stateMachine, bool isTwoHandsWeapon) : base(stateMachine)
    {
      this.isTwoHandsWeapon = isTwoHandsWeapon;
    }

    public override void Enter()
    {
      Debug.Log("Entramos en la animacion del pickUP");
      this.ChekShield();
      
      stateMachine.Health.SetInvulnerable(true);
      stateMachine.Animator.CrossFadeInFixedTime(PlayerPickupgHash, CrossFadeDuration);
      stateMachine.StartCoroutine(WaitForAnimationToEnd());
    }

    private void ChekShield()
    {
      stateMachine.SetIsTwoHandsWeapon(isTwoHandsWeapon);

      if(isTwoHandsWeapon)
      {
        stateMachine.Shield.SetActive(false);
      }
      else
      {
        stateMachine.Shield.SetActive(true);
      }
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        yield return new WaitForSeconds(2);
        // Instanciar y destruir los objetos correspondientes
        stateMachine.SwitchState(new WarriorPlayerFreeLookState(stateMachine)); 
    }

    public override void Tick(float deltaTime){ }

    public override void Exit()
    {
      stateMachine.Health.SetInvulnerable(false);
    }
}
