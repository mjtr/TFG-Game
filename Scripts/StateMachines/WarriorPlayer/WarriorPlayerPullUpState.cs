using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorPlayerPullUpState : WarriorPlayerBaseState
{
    private readonly int PullUpHash = Animator.StringToHash("PullUp");
    private readonly Vector3 Offset = new Vector3(0f, 2.325f,0.65f);
    private const float CrossFadeDuration = 0.1f;
    public WarriorPlayerPullUpState(WarriorPlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(PullUpHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(GetNormalizeTime(stateMachine.Animator, "Climbing") < 1f ){return;} //para comprobar si la animacion del pullUp ha terminado antes de pasar a la de vista libre
        
        stateMachine.Controller.enabled = false;
        stateMachine.transform.Translate(Offset,Space.Self);
        stateMachine.Controller.enabled = true;
       
        stateMachine.SwitchState(new WarriorPlayerFreeLookState(stateMachine,false));   
    }

    public override void Exit()
    {
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.ForceReceived.Reset();
        
    }


}
