using System.Collections;
using UnityEngine;

public class TrollScreamState : TrollBaseState
{

    private readonly int ScreamHash = Animator.StringToHash("Shout");

    private const float CrossFadeDuration = 0.1f;

    private float timeToWaitEndAnimation = 3.3f;

    public TrollScreamState(TrollStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.SetFirsTimeToSeePlayer();
        FacePlayer();
        stateMachine.DesactiveAllTrollWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.StartCoroutine(WaitForAnimationToEnd(ScreamHash, CrossFadeDuration));

    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new TrollChasingState(stateMachine));
    }

    public override void Tick(float deltaTime)
    { }

    public override void Exit()
    { 
        stateMachine.ResetNavhMesh();
    }
}
