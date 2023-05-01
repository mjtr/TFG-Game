using System.Collections;
using UnityEngine;

public class CrabMonsterScreamState : CrabMonsterBaseState
{

    private const float CrossFadeDuration = 0.1f;
    private float timeToWaitEndAnimation = 2.8f;
    public CrabMonsterScreamState(CrabMonsterStateMachine stateMachine) : base(stateMachine)
    { }

    public override void Enter()
    {
        string screamSelected = "Intimidate_1";
        int ScreamHash = Animator.StringToHash(screamSelected);
        stateMachine.SetFirsTimeToSeePlayer();
        FacePlayer();
        stateMachine.DesactiveAllCrabMonsterWeapon();
        stateMachine.isDetectedPlayed = true;
        stateMachine.StartCoroutine(WaitForAnimationToEnd(ScreamHash, CrossFadeDuration));
    }

    private IEnumerator WaitForAnimationToEnd(int animationHash, float transitionDuration)
    {
        stateMachine.Animator.CrossFadeInFixedTime(animationHash, transitionDuration);
        yield return new WaitForSeconds(timeToWaitEndAnimation);
        stateMachine.SwitchState(new CrabMonsterChasingState(stateMachine));
        
    }
    public override void Tick(float deltaTime)
    { }

    public override void Exit(){ 
       stateMachine.ResetNavhMesh();
    }
}
