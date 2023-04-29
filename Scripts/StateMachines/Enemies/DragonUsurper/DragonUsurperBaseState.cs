using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DragonUsurperBaseState : State
{

    protected DragonUsurperStateMachine stateMachine;

    public DragonUsurperBaseState(DragonUsurperStateMachine stateMachine)
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
        /* Para rotar poco a poco
        Quaternion targetRotation = Quaternion.LookRotation(lookPos);
        float rotationSpeed = 240f; // ajusta esta velocidad al gusto

        stateMachine.transform.rotation = Quaternion.RotateTowards(stateMachine.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        */

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPos);
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

    protected bool isInFrontOfPlayer()
    {
        if(stateMachine.PlayerHealth == null){ return false; }
        float dotThreshold = 0.65f;
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

    protected bool BlockAttackRandomize()
    {
        int num = Random.Range(0,20);
        if(num <= 6 ){
            return true;
        }     
       return false;
    }

}
