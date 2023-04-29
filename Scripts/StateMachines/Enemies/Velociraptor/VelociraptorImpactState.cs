using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VelociraptorImpactState : VelociraptorBaseState
{
    private string impactSelected;
    private float timeToWaitEndAnimation;
    private const float CrossFadeDuration = 0.1f;

    public VelociraptorImpactState(VelociraptorStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        GetRandomVelociraptorImpact();
        GetTimeToWaitAnimation();
        stateMachine.SetFirsTimeToSeePlayer();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllVelociraptorWeapon();

        stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(impactSelected), CrossFadeDuration));
        
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new VelociraptorChasingState(stateMachine));
    }

    private void GetRandomVelociraptorImpact()
    {
        stateMachine.EnableArmsDamage();
        int num = Random.Range(0,15);
        
        if(num <= 5 ){
            impactSelected = "Hit1";
            return;
        }

        if(num <= 10 ){
            impactSelected = "Hit2";
            return;
        }

        impactSelected = "Hit3";
    }

    private void GetTimeToWaitAnimation()
    {
        if(impactSelected == "Hit1"){
            timeToWaitEndAnimation = 1.3f;
            return;
        }
        
        if(impactSelected == "Hit2"){
            timeToWaitEndAnimation = 1.4f;
            return;
        }
        
        timeToWaitEndAnimation = 2.3f;
        return;
    }


    public override void Tick(float deltaTime)
    {}

    public override void Exit(){ 
        stateMachine.ResetNavhMesh();
    }
}
