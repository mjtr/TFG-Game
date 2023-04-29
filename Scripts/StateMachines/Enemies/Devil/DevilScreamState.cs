using UnityEngine;

public class DevilScreamState : DevilBaseState
{

    private readonly int ScreamHash = Animator.StringToHash("scream");

    private const float CrossFadeDuration = 0.1f;

    private float duration = 2.5f;

    public DevilScreamState(DevilStateMachine stateMachine) : base(stateMachine){  }
    public override void Enter()
    {
        stateMachine.SetFirsTimeToSeePlayer();
        FacePlayer();
        stateMachine.DesactiveAllDevilWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.Animator.CrossFadeInFixedTime(ScreamHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        
        duration -= deltaTime;
        if(duration <= 0f)
        {   
            stateMachine.SwitchState(new DevilChasingState(stateMachine));
        }
    }

    public override void Exit(){ 
       stateMachine.ResetNavhMesh();
    }
}
