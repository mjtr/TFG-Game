using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonUsurperDeadState : DragonUsurperBaseState
{
    private readonly int DragonUsurperDeadHash = Animator.StringToHash("Die");
    private const float CrossFadeDuration = 0.1f;
    public DragonUsurperDeadState(DragonUsurperStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDragonUsurperWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(DragonUsurperDeadHash, CrossFadeDuration);
        stateMachine.StartAmbientMusic();
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        GameObject.Destroy(stateMachine.Target);
        stateMachine.GetComponent<CharacterController>().enabled = false;
        stateMachine.DestroyCharacter(15f);
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
