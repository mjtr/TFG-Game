using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantWormDeadState : GiantWormBaseState
{
    private readonly int GiantWormDeadHash = Animator.StringToHash("Death");

    private const float CrossFadeDuration = 0.1f;
    public GiantWormDeadState(GiantWormStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.SetAudioControllerIsAttacking(false);
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopWormSounds();
        stateMachine.DesactiveAllWormWeapon();
        stateMachine.StopAllCourritines();
        stateMachine.Animator.CrossFadeInFixedTime(GiantWormDeadHash, CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        GameObject.Destroy(stateMachine.Target);
        stateMachine.StartAmbientMusic();
        stateMachine.StopParticlesEffects();
        stateMachine.GetComponent<CapsuleCollider>().enabled = false;
        
        if(stateMachine.GiantWormDeathBody != null)
        {
            stateMachine.StartCoroutine(WaitForAnimationToEnd());
        }else
        {
            stateMachine.DestroyCharacter(20f);
        }
       
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        AnimatorStateInfo animStateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

        while (animStateInfo.normalizedTime < 1.0f)
        {
            animStateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        stateMachine.InstanciateGiantWormDeathBody();
        stateMachine.DestroyCharacter();
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
