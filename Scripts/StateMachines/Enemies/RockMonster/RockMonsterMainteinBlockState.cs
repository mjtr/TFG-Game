using UnityEngine;

public class RockMonsterMainteinBlockState : RockMonsterBaseState
{

    private readonly int BlockHash = Animator.StringToHash("Block02");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 2.5f;

    public RockMonsterMainteinBlockState(RockMonsterStateMachine stateMachine) : base(stateMachine){  }
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
            stateMachine.SwitchState(new RockMonsterEndBlockState(stateMachine));
        }
    }

    public override void Exit(){ 
        stateMachine.Health.SetInvulnerable(false);
    }
}
