using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityDemonDeadState : InfinityDemonBaseState
{
    private readonly int InfinityDemonDeadHash = Animator.StringToHash("death");

    private const float CrossFadeDuration = 0.1f;
    public InfinityDemonDeadState(InfinityDemonStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.SetAudioControllerIsAttacking(false);
        stateMachine.DesactiveAllInfinityDemonWeapon();
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllInfinityDemonWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(InfinityDemonDeadHash, CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        stateMachine.StartAmbientMusic();
        GameObject.Destroy(stateMachine.Target);
        stateMachine.gameObject.GetComponent<CharacterController>().enabled = false;
        
        if(stateMachine.InfinityDemonDeathBody != null)
        {
            stateMachine.StartCoroutine(WaitForAnimationToEnd());
        }else
        {
            stateMachine.DestroyCharacterInTime(20f);
        }
        
    }

    private IEnumerator WaitForAnimationToEnd()
    {
         
        yield return new WaitForSeconds(2.4f);
        stateMachine.InstanciateInfinityDemonDeathBody();
        stateMachine.DestroyCharacter();
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
