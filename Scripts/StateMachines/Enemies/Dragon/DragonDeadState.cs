using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonDeadState : DragonBaseState
{
    private readonly int DragonDeadHash = Animator.StringToHash("Death");

    private const float CrossFadeDuration = 0.1f;
    public DragonDeadState(DragonStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopDragonSounds();
        stateMachine.isDetectedPlayed = false;
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDragonWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(DragonDeadHash, CrossFadeDuration);
        stateMachine.StartAmbientMusic();
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        GameObject.Destroy(stateMachine.Target);
        stateMachine.GetComponent<CharacterController>().enabled = false;
        stateMachine.DestroyCharacter(30f);
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
