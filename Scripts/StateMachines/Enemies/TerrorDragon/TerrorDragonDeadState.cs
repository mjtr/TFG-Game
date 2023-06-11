using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrorDragonDeadState : TerrorDragonBaseState
{
    private readonly int TerrorDragonDeadHash = Animator.StringToHash("Die");

    private const float CrossFadeDuration = 0.1f;
    public TerrorDragonDeadState(TerrorDragonStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.SetAudioControllerIsAttacking(false);
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllTerrorDragonWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(TerrorDragonDeadHash, CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        stateMachine.StartAmbientMusic();
        GameObject.Destroy(stateMachine.Target);
        stateMachine.GetComponent<CharacterController>().enabled = false;          
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
