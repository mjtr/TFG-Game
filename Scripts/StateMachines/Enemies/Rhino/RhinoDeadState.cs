using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoDeadState : RhinoBaseState
{
    private readonly int RhinoDeadHash = Animator.StringToHash("Dead");

    private const float CrossFadeDuration = 0.1f;
    public RhinoDeadState(RhinoStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllRhinoWeapon();
        stateMachine.isDetectedPlayed = false;
        stateMachine.Animator.CrossFadeInFixedTime(RhinoDeadHash, CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        stateMachine.StartAmbientMusic();
        stateMachine.gameObject.GetComponent<CharacterController>().enabled = false;
        GameObject.Destroy(stateMachine.Target);
        stateMachine.DestroyCharacter(20f);
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
