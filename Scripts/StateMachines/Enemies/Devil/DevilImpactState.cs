using UnityEngine;

public class DevilImpactState : DevilBaseState
{

    private readonly int ImpactHash = Animator.StringToHash("hit reaction");
    private const float CrossFadeDuration = 0.1f;
    private float duration = 1f;

    public DevilImpactState(DevilStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.StopParticlesEffects();
        stateMachine.StopAllCourritines();
        stateMachine.DesactiveAllDevilWeapon();
        stateMachine.StopSounds();
        stateMachine.Animator.CrossFadeInFixedTime(ImpactHash, CrossFadeDuration);  
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
        stateMachine.ResetNavhMesh();
    }
}
