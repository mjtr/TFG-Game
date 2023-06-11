using System.Collections;
using UnityEngine;

public class FarmAnimalEatingState : FarmAnimalBaseState
{
    private string EatingAnimation = "eat";
    private const float CrossFadeDuration = 0.1f;
    public FarmAnimalEatingState(FarmAnimalStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {
        int EatingHash = Animator.StringToHash(EatingAnimation);
        float newSpeedValue = Random.Range(0.2f,1.0f);
        stateMachine.Animator.SetFloat("Speed", newSpeedValue);
        stateMachine.Animator.CrossFadeInFixedTime(EatingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    { }

    public override void Exit(){ 
    }
}
