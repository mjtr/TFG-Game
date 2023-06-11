using System.Collections;
using UnityEngine;

public class VelociraptorWakeUpState : VelociraptorBaseState
{

    private string wakeUpAnimation = "WakeFromSleep";
    private const float CrossFadeDuration = 0.1f;
    public VelociraptorWakeUpState(VelociraptorStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {
        stateMachine.SetFirsTimeToSeePlayer();
        stateMachine.DesactiveAllVelociraptorWeapon();
        stateMachine.isDetectedPlayed = true;

        int wakeUpHash = Animator.StringToHash(wakeUpAnimation);
        float newSpeedValue = Random.Range(0.3f,1.2f);
        stateMachine.Animator.SetFloat("Speed", newSpeedValue);

        stateMachine.StartCoroutine(WaitForAnimationToEnd(wakeUpHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        
        AnimatorStateInfo stateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
        float originalDuration = stateInfo.length;
        float timeToWaitEndAnimation = originalDuration / stateInfo.speed;

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
