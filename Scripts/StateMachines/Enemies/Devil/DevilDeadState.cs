using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilDeadState : DevilBaseState
{
    private readonly int DevilDeadHash = Animator.StringToHash("death");

    private const float CrossFadeDuration = 0.1f;
    public DevilDeadState(DevilStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.SetAudioControllerIsAttacking(false);
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopSounds();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDevilWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(DevilDeadHash, CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        stateMachine.StartAmbientMusic();
        stateMachine.gameObject.GetComponent<CharacterController>().enabled = false;
        GameObject.Destroy(stateMachine.Target);
        stateMachine.DestroyCharacter(20f);
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
