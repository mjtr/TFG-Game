using UnityEngine;

public class RockMonsterRubbleState : RockMonsterBaseState
{

    private readonly int BlockHash = Animator.StringToHash("RubblePose");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 2.5f;

    public RockMonsterRubbleState(RockMonsterStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllRockMonsterWeapon();
        stateMachine.Health.SetInvulnerable(true);
        stateMachine.Animator.CrossFadeInFixedTime(BlockHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.StopParticlesEffects();
        if(IsInChaseRange())
        {
            stateMachine.SwitchState(new RockMonsterRubbleToIdleState(stateMachine));
            return;
        }
    }

    public override void Exit(){ }
}
