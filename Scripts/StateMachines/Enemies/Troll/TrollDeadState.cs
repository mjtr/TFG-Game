using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollDeadState : TrollBaseState
{
    private readonly int TrollDeadHash = Animator.StringToHash("Dead");

    private const float CrossFadeDuration = 0.1f;
    public TrollDeadState(TrollStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllTrollWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(TrollDeadHash, CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        stateMachine.StartAmbientMusic();
        GameObject.Destroy(stateMachine.Target);
        stateMachine.GetComponent<CharacterController>().enabled = false;
        stateMachine.DestroyCharacter(20f);
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
