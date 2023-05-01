using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrabMonsterHealState : CrabMonsterBaseState
{
    private string animationSelected = "Intimidate_3";
    private float timeToWaitEndAnimation = 14f;
    private const float CrossFadeDuration = 0.1f;

    public CrabMonsterHealState(CrabMonsterStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.SetFirsTimeToSeePlayer();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllCrabMonsterWeapon();

        stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(animationSelected), CrossFadeDuration));
        
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new CrabMonsterChasingState(stateMachine));
    }


    public override void Tick(float deltaTime)
    {
        stateMachine.Health.Heal(10f);

    }

    public override void Exit(){ 
        stateMachine.ResetNavhMesh();
    }
}
