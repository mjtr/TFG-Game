using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HumanPlayerBaseState : State
{
    protected HumanPlayerStateMachine stateMachine;

    public HumanPlayerBaseState(HumanPlayerStateMachine stateMachine){
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime){
        stateMachine.Controller.Move((motion + stateMachine.ForceReceived.Movement) * deltaTime);
    }

    protected void Move(float deltaTime){
        Move(Vector3.zero, deltaTime);
    }

    protected void FaceTarget(){
        if(stateMachine.Targeter.CurrentTarget == null){ return; }
        
        Vector3 lookPos = stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
        lookPos.y = 0;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected void ReturnToLocomotion()
    {
        if(stateMachine.Targeter.CurrentTarget != null){
            stateMachine.SwitchState(new HumanPlayerTargetingState(stateMachine));
        }else{
            stateMachine.SwitchState(new HumanPlayerFreeLookState(stateMachine));
        }

    }
}
