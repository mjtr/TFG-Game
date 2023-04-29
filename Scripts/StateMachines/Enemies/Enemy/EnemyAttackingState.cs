using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Attack");

    private const float TransitionDuration = 0.1f;


    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    { 
        //Antes de atacar girará 
        FacePlayer();
        stateMachine.Weapon.SetAttack(stateMachine.GetDamageStat(), stateMachine.AttackKnockback);
        
        stateMachine.Animator.CrossFadeInFixedTime(AttackHash, TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(GetNormalizeTime(stateMachine.Animator,"Attack") >= 1f)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
    //Si lo ponemos aqui el enemigo rotará a la vez que ataque, por lo que no se podrá esquivar a los lados
        //FacePlayer();
    }

    public override void Exit(){ }

}
