using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomDeadState : MushroomBaseState
{
    //private readonly int MushroomDeadHash = Animator.StringToHash("Death");

    private const float CrossFadeDuration = 0.1f;
    public MushroomDeadState(MushroomStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.SetAudioControllerIsAttacking(false);
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDemonWeapon();
        stateMachine.Animator.SetTrigger("death");
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        GameObject.Destroy(stateMachine.Target);
        stateMachine.StartAmbientMusic();
        stateMachine.GetComponent<CharacterController>().enabled = false;
        stateMachine.DestroyCharacter(20f);
    }

    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit(){ }
}
