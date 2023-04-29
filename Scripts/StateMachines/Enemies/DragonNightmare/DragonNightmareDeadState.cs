using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonNightmareDeadState : DragonNightmareBaseState
{
    private readonly int DragonNightmareDeadHash = Animator.StringToHash("die");
    private const float CrossFadeDuration = 0.1f;
    public DragonNightmareDeadState(DragonNightmareStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDragonNightmareWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(DragonNightmareDeadHash, CrossFadeDuration);
        stateMachine.StartAmbientMusic();
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        GameObject.Destroy(stateMachine.Target);
        stateMachine.GetComponent<CharacterController>().enabled = false;
        stateMachine.DestroyCharacter(60f);
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
