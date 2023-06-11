using System.Collections;
using UnityEngine;

public class VelociraptorSleepState : VelociraptorBaseState
{
    private string sleepAnimation = "SleepLoop";
    private const float CrossFadeDuration = 0.1f;
    public VelociraptorSleepState(VelociraptorStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {
        int SleepHash = Animator.StringToHash(sleepAnimation);
        stateMachine.SetFirsTimeToSeePlayer();
        stateMachine.DesactiveAllVelociraptorWeapon();
        float newSpeedValue = Random.Range(0.2f,1.3f);
        stateMachine.Animator.SetFloat("Speed", newSpeedValue);
        stateMachine.Animator.CrossFadeInFixedTime(SleepHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    { 
        if(isFromCallingAllies() || (stateMachine.isDetectedPlayed && stateMachine.isSleeping && !stateMachine.GetWakeUp()) || isInAttackRange())
        {
           stateMachine.SetFirsTimeToSeePlayer();
           stateMachine.isSleeping = false;
           stateMachine.SwitchState(new VelociraptorWakeUpState(stateMachine));
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
