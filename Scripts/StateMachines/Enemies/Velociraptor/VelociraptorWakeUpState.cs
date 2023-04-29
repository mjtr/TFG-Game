using System.Collections;
using UnityEngine;

public class VelociraptorWakeUpState : VelociraptorBaseState
{

    private string wakeUpAnimation = "WakeFromSleep";
    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 1.84f;
    public VelociraptorWakeUpState(VelociraptorStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {
        stateMachine.SetFirsTimeToSeePlayer();
        stateMachine.DesactiveAllVelociraptorWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(wakeUpAnimation), CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new VelociraptorChasingState(stateMachine));
        
    }
    public override void Tick(float deltaTime)
    { }

    public override void Exit(){ 
       stateMachine.ResetNavhMesh();
       stateMachine.SetWakeUp(false);
    }
}
