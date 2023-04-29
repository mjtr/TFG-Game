using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMonsterChargeMagicState : PlantMonsterBaseState
{
    private const float TransitionDuration = 0.1f;
    private readonly int PlantMonsterChargeMagicHash = Animator.StringToHash("Magic01charge");
    private float timeToWaitEndAnimation = 1f;
    public PlantMonsterChargeMagicState(PlantMonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {   
        stateMachine.Weapon.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.StartCoroutine(WaitForAnimationToEnd(PlantMonsterChargeMagicHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new PlantMonsterEndMagicState(stateMachine));
    }

    public override void Tick(float deltaTime)
    {}

    public override void Exit(){ }


}
