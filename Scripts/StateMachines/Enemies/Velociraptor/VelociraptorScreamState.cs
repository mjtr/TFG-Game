using System.Collections;
using UnityEngine;

public class VelociraptorScreamState : VelociraptorBaseState
{
    private string screamAnimation;
    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 2.4f;
    public VelociraptorScreamState(VelociraptorStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {
        GetRandomVelociraptorRoar();
        GetTimeToWaitAnimation();
        stateMachine.SetFirsTimeToSeePlayer();
        FacePlayer();
        stateMachine.DesactiveAllVelociraptorWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.StartCoroutine(WaitForAnimationToEnd(Animator.StringToHash(screamAnimation), CrossFadeDuration));
    }

    private void GetRandomVelociraptorRoar()
    {
        stateMachine.EnableArmsDamage();
        int num = Random.Range(0,10);
        if(num <= 5 ){
            screamAnimation = "Roar1";
            return;
        }
        screamAnimation = "Roar2";
    }

    private void GetTimeToWaitAnimation()
    {
        if(screamAnimation == "Roar1"){
            timeToWaitEndAnimation = 2.4f;
            return;
        }
        
        timeToWaitEndAnimation = 1.4f;
        return;
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new VelociraptorChasingState(stateMachine));
    }
     
    public override void Tick(float deltaTime)
    { }

    public override void Exit(){ 
       stateMachine.ResetNavhMesh();
    }
}
