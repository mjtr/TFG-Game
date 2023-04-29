using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlantMonsterBaseState : State
{

    protected PlantMonsterStateMachine stateMachine;

    public PlantMonsterBaseState(PlantMonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected bool IsInChaseRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;

        return playerDistanceSqr <= stateMachine.PlayerChasingRange * stateMachine.PlayerChasingRange;
    }

    protected void Move(Vector3 motion, float deltaTime){
        stateMachine.Controller.Move((motion + stateMachine.ForceReceived.Movement) * deltaTime);
    }

    protected void Move(float deltaTime){
        Move(Vector3.zero, deltaTime);
    }

    protected void FacePlayer()
    {
        if(stateMachine.PlayerHealth == null){ return; }
        
        Vector3 lookPos = stateMachine.PlayerHealth.transform.position - stateMachine.transform.position;
        lookPos.y = 0;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

}
