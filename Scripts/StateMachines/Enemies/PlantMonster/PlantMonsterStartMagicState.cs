using System.Collections;
using UnityEngine;

public class PlantMonsterStartMagicState : PlantMonsterBaseState
{
    private const float TransitionDuration = 0.1f;
    private readonly int PlantMonsterStartMagicHash = Animator.StringToHash("Magic01start");
    private float timeToWaitEndAnimation = 0.11f;
    public PlantMonsterStartMagicState(PlantMonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {   
        stateMachine.Weapon.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.StartCoroutine(WaitForAnimationToEnd(PlantMonsterStartMagicHash, TransitionDuration));
    }
    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new PlantMonsterChargeMagicState(stateMachine));
    }

    public override void Tick(float deltaTime)
    { }

    public override void Exit(){ }


}
