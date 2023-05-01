using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonDeadState : DemonBaseState
{
    private readonly int DemonDeadHash = Animator.StringToHash("Dead");

    private const float CrossFadeDuration = 0.1f;
    public DemonDeadState(DemonStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.SetAudioControllerIsAttacking(false);
        stateMachine.DesactiveAllDemonWeapon();
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllDemonWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(DemonDeadHash, CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        stateMachine.StartAmbientMusic();
        GameObject.Destroy(stateMachine.Target);
        stateMachine.gameObject.GetComponent<CharacterController>().enabled = false;
        
        if(stateMachine.DemonDeathBody != null)
        {
            stateMachine.StartCoroutine(WaitForAnimationToEnd());
        }else
        {
            stateMachine.DestroyCharacterInTime(20f);
        }
        
    }

    private IEnumerator WaitForAnimationToEnd()
    {
         
        yield return new WaitForSeconds(2.2f);

        // Instanciar y destruir los objetos correspondientes
        stateMachine.InstanciateDemonDeathBody();
        stateMachine.DestroyCharacter();
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
