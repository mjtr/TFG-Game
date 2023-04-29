using System.Collections;
using UnityEngine;

public class RhinoScreamState : RhinoBaseState
{
    private string screamAnimation = "shout";
    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 4.67f;
    public RhinoScreamState(RhinoStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {
        stateMachine.SetFirsTimeToSeePlayer();
        FacePlayer();
        stateMachine.DesactiveAllRhinoWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(screamAnimation), CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new RhinoChasingState(stateMachine));
    }
     
    public override void Tick(float deltaTime)
    { }

    public override void Exit(){ 
    }
}
