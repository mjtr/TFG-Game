using System.Collections;
using UnityEngine;

public class DragonUsurperScreamState : DragonUsurperBaseState
{

    private readonly int ScreamHash = Animator.StringToHash("Scream");
    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 3.35f;

    public DragonUsurperScreamState(DragonUsurperStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        FacePlayer();
        stateMachine.DesactiveAllDragonUsurperWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.StartCoroutine(WaitForAnimationToEnd(ScreamHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new DragonUsurperChasingState(stateMachine));
    }
    
    public override void Tick(float deltaTime)
    {}

    public override void Exit(){ }
}
