using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurDeadState : MinotaurBaseState
{
    private readonly int MinotaurDeadHash = Animator.StringToHash("death");

    private const float CrossFadeDuration = 0.1f;
    public MinotaurDeadState(MinotaurStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.SetAudioControllerIsAttacking(false);
        stateMachine.DesactiveAllMinotaurWeapon();
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllMinotaurWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(MinotaurDeadHash, CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        stateMachine.StartAmbientMusic();
        GameObject.Destroy(stateMachine.Target);
        stateMachine.gameObject.GetComponent<CharacterController>().enabled = false;
        
        if(stateMachine.MinotaurDeathBody != null)
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
        stateMachine.InstanciateMinotaurDeathBody();
        stateMachine.DestroyCharacter();
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
