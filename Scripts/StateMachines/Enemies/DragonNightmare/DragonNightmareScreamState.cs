using System.Collections;
using UnityEngine;

public class DragonNightmareScreamState : DragonNightmareBaseState
{

    private readonly int ScreamHash = Animator.StringToHash("Scream");
    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 2.9f;

    public DragonNightmareScreamState(DragonNightmareStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        FacePlayer();
        stateMachine.DesactiveAllDragonNightmareWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.StartCoroutine(WaitForAnimationToEnd(ScreamHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new DragonNightmareChasingState(stateMachine));
    }
    
    public override void Tick(float deltaTime)
    {}

    public override void Exit(){ }
}
