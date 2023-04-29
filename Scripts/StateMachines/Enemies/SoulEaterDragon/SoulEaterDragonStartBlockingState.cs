using UnityEngine;

public class SoulEaterDragonStartBlockingState : SoulEaterDragonBaseState
{

    private readonly int BlockHash = Animator.StringToHash("DefendStart");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 2.5f;

    public SoulEaterDragonStartBlockingState(SoulEaterDragonStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        FacePlayer();
        stateMachine.DesactiveAllSoulEaterDragonWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.Health.SetInvulnerable(true);
        stateMachine.Animator.CrossFadeInFixedTime(BlockHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        duration -= deltaTime;
        if(duration <= 0f)
        {   
            stateMachine.SwitchState(new SoulEaterDragonEndBlockingState(stateMachine));
        }
    }

    public override void Exit(){ 
        stateMachine.Health.SetInvulnerable(false);
    }
}
