using System.Collections;
using UnityEngine;

public class RhinoEatingState : RhinoBaseState
{
    private string EatingAnimation = "Eats";
    private const float CrossFadeDuration = 0.1f;
    public RhinoEatingState(RhinoStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {
        int EatingHash = Animator.StringToHash(EatingAnimation);
        stateMachine.SetFirsTimeToSeePlayer();
        stateMachine.DesactiveAllRhinoWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(EatingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    { 
        if((stateMachine.isDetectedPlayed && stateMachine.isEating) || isInAttackRange())
        {
           stateMachine.SetFirsTimeToSeePlayer();
           stateMachine.isEating = false;
           stateMachine.SwitchState(new RhinoChasingState(stateMachine));
        }

    }


    private bool isInAttackRange()
    {
        if(stateMachine.PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (stateMachine.PlayerHealth.transform.position - stateMachine.transform.position).sqrMagnitude;
   
        return playerDistanceSqr <= stateMachine.AttackRange * stateMachine.AttackRange;
    }

    public override void Exit(){ 
    }
}
