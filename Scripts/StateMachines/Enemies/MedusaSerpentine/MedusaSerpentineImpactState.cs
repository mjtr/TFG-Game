using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaSerpentineImpactState : MedusaSerpentineBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("GotHit");
    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 1.47f;

    public MedusaSerpentineImpactState(MedusaSerpentineStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.StartCoroutine(WaitForAnimationToEnd(ImpactHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new MedusaSerpentineChasingState(stateMachine));
    }


    public override void Tick(float deltaTime){
        stateMachine.AddTimeToScreamTime(deltaTime);    
     }

    public override void Exit(){
        stateMachine.ResetNavMesh();
    }
}
