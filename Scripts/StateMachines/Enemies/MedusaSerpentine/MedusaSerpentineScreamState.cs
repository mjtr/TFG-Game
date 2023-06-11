using System.Collections;
using UnityEngine;

public class MedusaSerpentineScreamState : MedusaSerpentineBaseState
{
    private string screamAnimation = "Taunt";
    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 3.77f;
    public MedusaSerpentineScreamState(MedusaSerpentineStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {
        FacePlayer();
        stateMachine.DesactiveAllMedusaSerpentineWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(screamAnimation), CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new MedusaSerpentineChasingState(stateMachine));
    }
     
    public override void Tick(float deltaTime)
    { }

    public override void Exit(){ 
    }
}
