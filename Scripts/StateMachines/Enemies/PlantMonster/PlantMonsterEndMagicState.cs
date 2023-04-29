using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMonsterEndMagicState : PlantMonsterBaseState
{
    private const float TransitionDuration = 0.1f;
    private readonly int PlantMonsterEndMagicHash = Animator.StringToHash("Magic01end");
    private float timeToWaitEndAnimation = 1.15f;
    public PlantMonsterEndMagicState(PlantMonsterStateMachine stateMachine) : base(stateMachine) {    }
    public override void Enter()
    {   
        FacePlayer();
        stateMachine.Weapon.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.StartCoroutine(WaitForAnimationToEnd(PlantMonsterEndMagicHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new PlantMonsterChasingState(stateMachine));
    }

    public override void Tick(float deltaTime)
    { }

    public override void Exit(){

     }

}
