using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonDogDeadState : DemonDogBaseState
{
    private readonly int DemonDogDeadHash = Animator.StringToHash("Death");

    private const float CrossFadeDuration = 0.1f;
    public DemonDogDeadState(DemonDogStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.SetAudioControllerIsAttacking(false);
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDemonDogWeapon();
        stateMachine.isDetectedPlayed = false;
        stateMachine.Animator.CrossFadeInFixedTime(DemonDogDeadHash, CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        stateMachine.StartAmbientMusic();
        stateMachine.gameObject.GetComponent<CharacterController>().enabled = false;
        GameObject.Destroy(stateMachine.Target);
        stateMachine.DestroyCharacter(20f);
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
