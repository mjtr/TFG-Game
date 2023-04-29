using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantMonsterImpactState : PlantMonsterBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("GotHit");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public PlantMonsterImpactState(PlantMonsterStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        if(MustProduceGetHitAnimation())
        {
            stateMachine.StopAllCourritines();
            stateMachine.DesactiveAllPlantMonsterWeapon();
            stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);
        }
    }

    private bool MustProduceGetHitAnimation()
    {
        int num = Random.Range(0,20);
        if(num <= 6 ){
            return false;
        }     
        return true;
    }

    public override void Tick(float deltaTime)
    {
        if(!stateMachine.isOnlySpellMagic){
            Move(deltaTime);
        }

        duration -= deltaTime;
        if(duration <= 0f)
        {   
            stateMachine.SwitchState(new PlantMonsterIdleState(stateMachine));
        }
    }

    public override void Exit(){ }
}
