using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrabMonsterImpactState : CrabMonsterBaseState
{
    private string impactSelected;
    private float timeToWaitEndAnimation;
    private const float CrossFadeDuration = 0.1f;

    public CrabMonsterImpactState(CrabMonsterStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        GetRandomCrabMonsterImpact();
        GetTimeToWaitAnimation();
        stateMachine.SetFirsTimeToSeePlayer();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllCrabMonsterWeapon();

        stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(impactSelected), CrossFadeDuration));
        
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new CrabMonsterChasingState(stateMachine));
    }

    private void GetRandomCrabMonsterImpact()
    {
        stateMachine.EnableArmsDamage();
        int num = Random.Range(0,15);
        
        if(num <= 5 ){
            impactSelected = "Take_Damage_1";
            return;
        }

        if(num <= 10 ){
            impactSelected = "Take_Damage_2";
            return;
        }

        impactSelected = "Take_Damage_3";
    }

    private void GetTimeToWaitAnimation()
    {
        if(impactSelected == "Take_Damage_1"){
            timeToWaitEndAnimation = 1.1f;
            return;
        }
        
        if(impactSelected == "Take_Damage_2"){
            timeToWaitEndAnimation = 1.2f;
            return;
        }
        
        timeToWaitEndAnimation = 2.0f;
        return;
    }


    public override void Tick(float deltaTime)
    {}

    public override void Exit(){ 
        stateMachine.ResetNavhMesh();
    }
}
