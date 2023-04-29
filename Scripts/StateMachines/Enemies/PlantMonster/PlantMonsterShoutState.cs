using System.Collections;
using UnityEngine;

public class PlantMonsterShoutState : PlantMonsterBaseState
{
    private const float TransitionDuration = 0.1f;
    private readonly int PlantMonsterShoutHash = Animator.StringToHash("AttackIdleBreak");
    private float timeToWaitEndAnimation = 5.18f;
    public PlantMonsterShoutState(PlantMonsterStateMachine stateMachine) : base(stateMachine) {    }

    public override void Enter()
    {   
        FacePlayer();
        stateMachine.StartCoroutine(WaitForAnimationToEnd(PlantMonsterShoutHash, TransitionDuration));
    }
    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);

        if(IsInChaseRange())
        {
            stateMachine.SwitchState(new PlantMonsterChasingState(stateMachine));
        }else
        {
            stateMachine.SwitchState(new PlantMonsterSleepState(stateMachine));
        }
       
    }

    public override void Tick(float deltaTime)
    { }

    public override void Exit(){ }


}
