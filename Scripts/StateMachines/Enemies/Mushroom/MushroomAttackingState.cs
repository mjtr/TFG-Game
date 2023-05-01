using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomAttackingState : MushroomBaseState
{
    private const float TransitionDuration = 0.1f;

    private float timeToWaitEndAnimation;
    private bool onlyMagic;

    public MushroomAttackingState(MushroomStateMachine stateMachine, bool onlyMagic) : base(stateMachine)
    {
        this.onlyMagic = onlyMagic;
    }
    public override void Enter()
    {   
        string attackChoosed = GetRandomMushroomAttack();
        int AttackHash = Animator.StringToHash(attackChoosed);
        FacePlayer();
        stateMachine.Weapon.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        stateMachine.StartCoroutine(WaitForAnimationToEnd(AttackHash, TransitionDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new MushroomChasingState(stateMachine));
    }

    public override void Tick(float deltaTime) { }

    public override void Exit(){
        stateMachine.Agent.enabled = true;
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.enabled = false;
        stateMachine.Agent.enabled = true;
    }

    private string GetRandomMushroomAttack()
    {
        if(onlyMagic)
        {
            timeToWaitEndAnimation = 3f;
            return "MushAttack02";
        }

        int num = Random.Range(0,15);
        if(num <= 5 ){
            timeToWaitEndAnimation = 2f;
            return "MushAttack01";

        }else if (num <= 10)
        {
            timeToWaitEndAnimation = 3f;
            return "MushAttack02";   

        }else{
            timeToWaitEndAnimation = 1.7f;
            return "MushAttack03";
        }
    }

}
