using System.Collections;
using UnityEngine;

public class VelociraptorCallingState : VelociraptorBaseState
{
    private string callAnimation = "Call1";
    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 2.6f;
    public VelociraptorCallingState(VelociraptorStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {
        stateMachine.GetWarriorPlayerStateMachine().SetVelociraptorCallingAllies(true);
        stateMachine.DesactiveAllVelociraptorWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(callAnimation), CrossFadeDuration));
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
       stateMachine.GetWarriorPlayerStateMachine().SetVelociraptorCallingAllies(false);
    }
}
