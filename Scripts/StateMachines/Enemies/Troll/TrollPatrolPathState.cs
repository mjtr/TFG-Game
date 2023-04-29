using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollPatrolPathState : TrollBaseState
{

   GameObject player;
    Vector3 guardPosition;
    float timeSinceLastSawPlayer = 0f;
    float timeSinceArriveWaypoint = Mathf.Infinity;
    private int timeToResetNavMesh = 0;
    int currentWaypointIndex = 0;
    bool noPatrolPathFirstCall = true;
    private const float AnimatorDampTime = 0.1f;
    private readonly int LocomotionBlendTreeHash = Animator.StringToHash("Locomotion");
    private readonly int LocomotionHash = Animator.StringToHash("locomotion");
    public TrollPatrolPathState(TrollStateMachine stateMachine) : base(stateMachine){  }

    public override void Enter()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        guardPosition = stateMachine.transform.position;   
        stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash, AnimatorDampTime);
    }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.Health.CheckIsDead()) { return; }
        if(stateMachine.PatrolPath == null)
        {
            if(noPatrolPathFirstCall){
                stateMachine.SwitchState(new TrollIdleState(stateMachine));
                noPatrolPathFirstCall = false;
            }
            return;
        }

        if (IsInChaseRange() && isInFrontOfPlayer())
        {
            timeSinceLastSawPlayer = 0f;
            if(stateMachine.GetFirsTimeTotSeePlayer())
            {
                stateMachine.SwitchState(new TrollScreamState(stateMachine));
            }else{
                stateMachine.SwitchState(new TrollChasingState(stateMachine));
            }
            
        }
        else if (timeSinceLastSawPlayer < stateMachine.SuspiciousTime)
        {
            SuspiciousBehaviour();
        }
        else
        {
            PatrolBehaviour(deltaTime);
        }

        ResetNavMesh();
        
        UpdateTimers();
    }

    private void ResetNavMesh()
    {
        timeToResetNavMesh ++;
        if(timeToResetNavMesh > 300)
        {
            timeToResetNavMesh = 0;
            stateMachine.Agent.ResetPath();
            stateMachine.Agent.enabled = false;
            stateMachine.Agent.enabled = true;
        }
    }

    public override void Exit() { 
        stateMachine.ResetNavhMesh();
    }

    private void UpdateTimers()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceArriveWaypoint += Time.deltaTime;
    }

    //El patrullar. Si no está el objeto metido hará guardia, pero si está se moverá
    private void PatrolBehaviour(float deltaTime)
    {
        
        if(AtWaypoint())
        {
            stateMachine.Animator.SetFloat(LocomotionHash,0f);
            timeSinceArriveWaypoint = 0;
            CycleWaypoint();
        }

        Vector3 nextPosition = GetCurrentWaypoint();
        
        if(timeSinceArriveWaypoint > stateMachine.WaypointDwellTime)
        {
            MoveToNextPosition(nextPosition, stateMachine.PatrolSpeedFraction * deltaTime);
        }
    }

    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(stateMachine.transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < stateMachine.WaypointTolerance;
    }


    private Vector3 GetCurrentWaypoint()
    {
        return stateMachine.PatrolPath.GetWaypoint(currentWaypointIndex);
    }

    private void CycleWaypoint()
    {
        currentWaypointIndex = stateMachine.PatrolPath.GetNextIndex(currentWaypointIndex);
    }

    
    private void SuspiciousBehaviour()
    {
        stateMachine.Animator.SetFloat(LocomotionHash,0f);
    }

    //Call by Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(stateMachine.transform.position, stateMachine.ChaseDistance);
    }

    private void MoveToNextPosition(Vector3 destination, float deltaTime)
    {   
        stateMachine.Animator.SetFloat(LocomotionHash,0.5f, AnimatorDampTime, deltaTime);

        if(stateMachine.Agent.isOnNavMesh)
        {
            MoveToWaitPoint(destination,deltaTime);
        }
    
    }

}
