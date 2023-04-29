using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RhinoBaseState : State
{

    protected RhinoStateMachine stateMachine;

    public RhinoBaseState(RhinoStateMachine stateMachine)
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

    protected bool isInFrontOfPlayer()
    {
        if(stateMachine.PlayerHealth == null){ return false; }
        float dotThreshold = 0.6f;
        Vector3 enemyDirection = stateMachine.transform.forward;

        // Calcular la dirección hacia el personaje principal
        Vector3 toTarget = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).normalized;

        // Comparar las direcciones utilizando el producto punto
        float dotProduct = Vector3.Dot(enemyDirection, toTarget);
        if (dotProduct > dotThreshold)
        {
            // El enemigo está mirando hacia el personaje principal
            stateMachine.isDetectedPlayed = true;
            return true;
        }
        return false;
    }

    protected void FaceWaypoint(Vector3 waitpointTarget)
    {
        Vector3 lookPos = waitpointTarget - stateMachine.transform.position;
        lookPos.y = 0;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
    }

    protected void MoveToWaitPoint(Vector3 destination, float deltaTime)
    {   

        if(stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.destination = destination;
            stateMachine.Agent.speed = stateMachine.MaxSpeed * Mathf.Clamp01(stateMachine.PatrolSpeedFraction);
            stateMachine.Agent.isStopped = false;
            FaceWaypoint(destination);
            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.Agent.speed,deltaTime);
        }

        stateMachine.Agent.velocity = stateMachine.Controller.velocity;
    }

}
