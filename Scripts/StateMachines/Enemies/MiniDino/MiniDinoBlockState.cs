using UnityEngine;

public class MiniDinoBlockState : MiniDinoBaseState
{

    private readonly int BlockHash = Animator.StringToHash("Block");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 2.5f;

    public MiniDinoBlockState(MiniDinoStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        FacePlayer();
        stateMachine.DesactiveAllMiniDinoWeapon();
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
            stateMachine.SwitchState(new MiniDinoIdleState(stateMachine));
        }
    }

    public override void Exit(){ 
        stateMachine.Health.SetInvulnerable(false);
    }
}
