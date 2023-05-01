using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelociraptorDeadState : VelociraptorBaseState
{
    private string deathSelected;
    private const float CrossFadeDuration = 0.1f;
    public VelociraptorDeadState(VelociraptorStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.SetAudioControllerIsAttacking(false);
        GetRandomVelociraptorDeath();
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllVelociraptorWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(Animator.StringToHash(deathSelected), CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        stateMachine.StartAmbientMusic();
        stateMachine.gameObject.GetComponent<CharacterController>().enabled = false;
        GameObject.Destroy(stateMachine.Target);
    }

    private void GetRandomVelociraptorDeath()
    {
        stateMachine.EnableArmsDamage();
        int num = Random.Range(0,10);
        if(num <= 5 ){
            deathSelected = "Death1";
            return;
        }
        deathSelected = "Death2";
    }


    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
