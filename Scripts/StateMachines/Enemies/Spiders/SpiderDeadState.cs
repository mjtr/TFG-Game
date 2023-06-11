using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderDeadState : SpiderBaseState
{
    private string deathSelected  = "Death 1";
    private const float CrossFadeDuration = 0.1f;
    public SpiderDeadState(SpiderStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.SetAudioControllerIsAttacking(false);
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllSpiderWeapon();
        GetRandomSpiderDeath();
        stateMachine.Animator.CrossFadeInFixedTime(Animator.StringToHash(deathSelected), CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        GameObject.Destroy(stateMachine.Target);
        stateMachine.StartAmbientMusic();
        stateMachine.GetComponent<CharacterController>().enabled = false;
        stateMachine.DestroyCharacter(20f);
    }
    
    private void GetRandomSpiderDeath()
    {
        int num = Random.Range(0,10);
        if(num <= 5 ){
            deathSelected = "Death 1";
            return;
        }
        deathSelected = "Death 2";
    }


    public override void Tick(float deltaTime)
    {
        
    }

    public override void Exit(){ }
}
