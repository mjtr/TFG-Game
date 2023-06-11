using System.Collections;
using UnityEngine;

public class DragonNightmareImpactState : DragonNightmareBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("getHit");
    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 1.41f;

    public DragonNightmareImpactState(DragonNightmareStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDragonNightmareWeapon();
        stateMachine.StartCoroutine(WaitForAnimationToEnd(ImpactHash, CrossFadeDuration));
    }
    
    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new DragonNightmareIdleState(stateMachine));
    }

    public override void Tick(float deltaTime){ }

    public override void Exit(){ 
        stateMachine.ResetNavhMesh();
    }
}
