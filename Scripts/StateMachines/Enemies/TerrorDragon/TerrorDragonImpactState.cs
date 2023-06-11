using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrorDragonImpactState : TerrorDragonBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("Get Hit");

    private const float CrossFadeDuration = 0.1f;

    public TerrorDragonImpactState(TerrorDragonStateMachine stateMachine) : base(stateMachine){  }

    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.DesactiveAllTerrorDragonWeapon();
        stateMachine.StartCoroutine(WaitForAnimationToEnd(ImpactHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(1.3f);
        stateMachine.SwitchState(new TerrorDragonChasingState(stateMachine));
        
    }

    public override void Tick(float deltaTime)
    {     
        stateMachine.AddTimeToScreamTime(deltaTime);      
    }

    public override void Exit(){
        stateMachine.ResetNavhMesh();
     }
}
