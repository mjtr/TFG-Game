using System.Collections;
using UnityEngine;

public class TerrorDragonScreamState : TerrorDragonBaseState
{
    private string screamAnimation = "Scream";
    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 3f;
    public TerrorDragonScreamState(TerrorDragonStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {
        FacePlayer();
        stateMachine.DesactiveAllTerrorDragonWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(screamAnimation), CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new TerrorDragonChasingState(stateMachine));
    }
     
    public override void Tick(float deltaTime)
    { }

    public override void Exit(){ 
    }
}
