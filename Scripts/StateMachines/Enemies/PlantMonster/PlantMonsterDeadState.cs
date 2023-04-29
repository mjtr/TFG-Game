using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMonsterDeadState : PlantMonsterBaseState
{
    private readonly int PlantMonsterDeadHash = Animator.StringToHash("Death");

    private const float CrossFadeDuration = 0.1f;
    public PlantMonsterDeadState(PlantMonsterStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.StartAmbientMusic();
        stateMachine.DesactiveAllPlantMonsterWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(PlantMonsterDeadHash, CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        GameObject.Destroy(stateMachine.Target);
        stateMachine.GetComponent<CharacterController>().enabled = false;
        stateMachine.DestroyCharacter(20f);
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
