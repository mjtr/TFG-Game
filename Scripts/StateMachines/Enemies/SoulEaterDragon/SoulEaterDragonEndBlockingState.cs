using UnityEngine;

public class SoulEaterDragonEndBlockingState : SoulEaterDragonBaseState
{

    private readonly int BlockHash = Animator.StringToHash("DefendEnd");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public SoulEaterDragonEndBlockingState(SoulEaterDragonStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.isDetectedPlayed = true;
        stateMachine.Animator.CrossFadeInFixedTime(BlockHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        duration -= deltaTime;
        if(duration <= 0f)
        {   
            stateMachine.SwitchState(new SoulEaterDragonIdleState(stateMachine));
        }
    }

    public override void Exit(){ 
        stateMachine.Health.SetInvulnerable(false);
    }
}
