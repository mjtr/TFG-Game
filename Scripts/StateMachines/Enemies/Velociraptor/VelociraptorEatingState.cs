using System.Collections;
using UnityEngine;

public class VelociraptorEatingState : VelociraptorBaseState
{
    private string EatingAnimation = "EatPrey";
    private const float CrossFadeDuration = 0.1f;
    public VelociraptorEatingState(VelociraptorStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {
        int EatingHash = Animator.StringToHash(EatingAnimation);
        stateMachine.SetFirsTimeToSeePlayer();
        stateMachine.DesactiveAllVelociraptorWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(EatingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    { 
        if((stateMachine.isDetectedPlayed && stateMachine.isEating) || isInAttackRange() || isFromCallingAllies())
        {
           stateMachine.SetFirsTimeToSeePlayer();
           stateMachine.isEating = false;
           stateMachine.SwitchState(new VelociraptorChasingState(stateMachine));
        }

    }

     private bool isFromCallingAllies()
    {
        return  IsInChaseRange() && stateMachine.GetWarriorPlayerStateMachine().GetVelociraptorCallingAllies();
    }

    private bool isInAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }

    public override void Exit(){ 
       stateMachine.ResetNavhMesh();
       stateMachine.SetWakeUp(true);
    }
}
