using UnityEngine;

public class RockMonsterBlockState : RockMonsterBaseState
{

    private readonly int BlockHash = Animator.StringToHash("Block01");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1.5f;

    public RockMonsterBlockState(RockMonsterStateMachine stateMachine) : base(stateMachine){  }
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
            stateMachine.SwitchState(new RockMonsterMainteinBlockState(stateMachine));
        }
    }

    public override void Exit(){ 
        stateMachine.Health.SetInvulnerable(false);
    }
}
