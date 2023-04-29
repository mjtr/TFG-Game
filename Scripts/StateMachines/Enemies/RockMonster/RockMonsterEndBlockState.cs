using UnityEngine;

public class RockMonsterEndBlockState : RockMonsterBaseState
{

    private readonly int BlockHash = Animator.StringToHash("Block03");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public RockMonsterEndBlockState(RockMonsterStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        FacePlayer();
        stateMachine.DesactiveAllRockMonsterWeapon();
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
            stateMachine.SwitchState(new RockMonsterIdleState(stateMachine));
        }
    }

    public override void Exit(){ 
        stateMachine.Health.SetInvulnerable(false);
    }
}
