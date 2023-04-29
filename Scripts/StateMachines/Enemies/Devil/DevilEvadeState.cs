using UnityEngine;

public class DevilEvadeState : DevilBaseState
{

    private const float CrossFadeDuration = 0.1f;

    private float duration = 1f;

    public DevilEvadeState(DevilStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        FacePlayer();
        stateMachine.DesactiveAllDevilWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.Health.SetInvulnerable(true);
        stateMachine.Animator.CrossFadeInFixedTime(getRandomEvadeHash(), CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        duration -= deltaTime;
        if(duration <= 0f)
        {   
            stateMachine.SwitchState(new DevilIdleState(stateMachine));
        }
    }

    public override void Exit(){ 
        stateMachine.Health.SetInvulnerable(false);
    }

    private int getRandomEvadeHash()
    {
        int num = Random.Range(0,10);
        if(num <= 5 ){
            return Animator.StringToHash("hop left");
        }
       return Animator.StringToHash("hop right");
    }
}
