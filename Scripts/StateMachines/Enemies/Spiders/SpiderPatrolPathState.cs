using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderPatrolPathState : SpiderBaseState
{

   GameObject player;
    Vector3 guardPosition;// La posición inicial la cual volverá después el enemigo una vez deja de seguirte
 //   DiabloMover mover;
    float timeSinceLastSawPlayer = 0f;
    float timeSinceArriveWaypoint = Mathf.Infinity;
    private int timeToResetNavMesh = 0;
    int currentWaypointIndex = 0;
    bool noPatrolPathFirstCall = true;
    private readonly int SpeedHash = Animator.StringToHash("locomotion");
    private const float AnimatorDampTime = 0.1f;

    public SpiderPatrolPathState(SpiderStateMachine stateMachine) : base(stateMachine){  }

    public override void Enter()
    {
        stateMachine.SetAudioControllerIsAttacking(false);
        player = GameObject.FindGameObjectWithTag("Player");
        guardPosition = stateMachine.transform.position; }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.Health.CheckIsDead()) { return; }
        if(stateMachine.PatrolPath == null)
        {
            if(noPatrolPathFirstCall){
                stateMachine.SwitchState(new SpiderIdleState(stateMachine));
                noPatrolPathFirstCall = false;
            }
            return;
        }

        if (IsInChaseRange() && isInFrontOfPlayer())
        {
            timeSinceLastSawPlayer = 0f;
            stateMachine.SwitchState(new SpiderChasingState(stateMachine));
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

    public override void Exit() {
        stateMachine.ResetNavMesh();
    }

    private void UpdateTimers()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceArriveWaypoint += Time.deltaTime;
    }

     private void ResetNavMesh()
    {
        timeToResetNavMesh ++;
        if(timeToResetNavMesh > 300)
        {
            timeToResetNavMesh = 0;
            stateMachine.ResetNavMesh();
        }
    }

    //El patrullar. Si no está el objeto metido hará guardia, pero si está se moverá
    private void PatrolBehaviour(float deltaTime)
    {
        
        if(AtWaypoint())
        {
            stateMachine.Animator.SetFloat(SpeedHash,0f);
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
        stateMachine.Animator.SetFloat(SpeedHash,0f);
    }

    //Call by Unity
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(stateMachine.transform.position, stateMachine.ChaseDistance);
    }

    private void MoveToNextPosition(Vector3 destination, float deltaTime)
    {   
        stateMachine.Animator.SetFloat(SpeedHash,1f, AnimatorDampTime, deltaTime);

        if(stateMachine.Agent.isOnNavMesh)
        {
            MoveToWaitPoint(destination,deltaTime);
        }
    
    }

}
