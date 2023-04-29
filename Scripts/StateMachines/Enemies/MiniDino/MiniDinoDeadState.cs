using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniDinoDeadState : MiniDinoBaseState
{
    private readonly int MiniDinoDeadHash = Animator.StringToHash("Death");

    private const float CrossFadeDuration = 0.1f;
    public MiniDinoDeadState(MiniDinoStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllMiniDinoWeapon();
        stateMachine.StartAmbientMusic();
        stateMachine.Animator.CrossFadeInFixedTime(MiniDinoDeadHash, CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        GameObject.Destroy(stateMachine.Target);
        stateMachine.GetComponent<CharacterController>().enabled = false;
        stateMachine.DestroyCharacter(20f);
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
