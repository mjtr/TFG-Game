using System.Collections;
using UnityEngine;

public class PlantMonsterSleepState : PlantMonsterBaseState
{
    private const float TransitionDuration = 0.1f;
    private readonly int PlantMonsterSleepHash = Animator.StringToHash("GoToSleep");
    private float timeToWaitEndAnimation = 3.05f;
    public PlantMonsterSleepState(PlantMonsterStateMachine stateMachine) : base(stateMachine) {    }

    public override void Enter()
    {   
        stateMachine.StartCoroutine(WaitForAnimationToEnd(PlantMonsterSleepHash, TransitionDuration));
    }
    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new PlantMonsterIdleState(stateMachine));
    }

    public override void Tick(float deltaTime)
    { }

    public override void Exit(){ }


}
