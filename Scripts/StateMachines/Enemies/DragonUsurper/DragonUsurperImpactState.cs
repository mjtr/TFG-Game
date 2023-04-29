using System.Collections;
using UnityEngine;

public class DragonUsurperImpactState : DragonUsurperBaseState
{
    private readonly int ImpactHash = Animator.StringToHash("Get Hit");
    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 1.35f;

    public DragonUsurperImpactState(DragonUsurperStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDragonUsurperWeapon();
        stateMachine.StartCoroutine(WaitForAnimationToEnd(ImpactHash, CrossFadeDuration));
    }
    
    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new DragonUsurperIdleState(stateMachine));
    }

    public override void Tick(float deltaTime){ }

    public override void Exit(){ }
}
