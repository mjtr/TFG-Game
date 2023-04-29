using UnityEngine;

public class RockMonsterRubbleToIdleState : RockMonsterBaseState
{

    private readonly int RubbleToIdleHash = Animator.StringToHash("RubbleToIdle");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 5f;

    public RockMonsterRubbleToIdleState(RockMonsterStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.StopSounds();
        stateMachine.DesactiveAllRockMonsterWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.Health.SetInvulnerable(true);
        stateMachine.Animator.CrossFadeInFixedTime(RubbleToIdleHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        duration -= deltaTime;
        if(duration <= 0f)
        {   
            stateMachine.SwitchState(new RockMonsterIdleState(stateMachine));
        }
    }

    public override void Exit(){ 
        stateMachine.Health.SetInvulnerable(false);
    }
}
