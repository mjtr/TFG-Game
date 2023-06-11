using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderIdleState : SpiderBaseState
{

    private readonly int StatueBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int StatueHash = Animator.StringToHash("locomotion");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;

    private float randomIdle;

    public SpiderIdleState(SpiderStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        GetRandomSpiderIdle();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllSpiderWeapon();
        stateMachine.Animator.SetFloat(StatueHash, randomIdle);
        stateMachine.Animator.CrossFadeInFixedTime(StatueBlendTreeHash, CrossFadeDuration);
    }

    private void GetRandomSpiderIdle()
    {
        int num = Random.Range(0,10);
        if(num <= 5 ){
            randomIdle = 0f;
            return;
        }
        randomIdle = 0.5f;
    }


    public override void Tick(float deltaTime)
    {

        Move(deltaTime);

        if(IsInChaseRange() && (isInFrontOfPlayer() || stateMachine.isDetectedPlayed))
        {
            stateMachine.SwitchState(new SpiderChasingState(stateMachine));
            return;
        }

    }

    public override void Exit(){ }


}
