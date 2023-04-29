using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMonsterAttackingState : PlantMonsterBaseState
{
    private const float TransitionDuration = 0.2f;

    private float timeToWaitEndAnimation;

    public PlantMonsterAttackingState(PlantMonsterStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {   
        string attackChoosed = GetRandomPlantMonsterAttack();
        int AttackHash = Animator.StringToHash(attackChoosed);
        FacePlayer();
        stateMachine.Weapon.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }
    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new PlantMonsterChasingState(stateMachine));
    }
    public override void Tick(float deltaTime) { }

    public override void Exit(){ }

    private string GetRandomPlantMonsterAttack()
    {
        int num = Random.Range(0,10);
        if(num <= 5 ){
            timeToWaitEndAnimation = 1.03f;
            return "Attack01";
        }else{
            timeToWaitEndAnimation = 2.2f;
            return "Attack02";
        }
    }

}
