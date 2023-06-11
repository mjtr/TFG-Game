using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaSerpentineDeadState : MedusaSerpentineBaseState
{
    private readonly int MedusaSerpentineDeadHash = Animator.StringToHash("Death");

    private const float CrossFadeDuration = 0.1f;
    public MedusaSerpentineDeadState(MedusaSerpentineStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.SetAudioControllerIsAttacking(false);
        stateMachine.DesactiveAllMedusaSerpentineWeapon();
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllMedusaSerpentineWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(MedusaSerpentineDeadHash, CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        stateMachine.StartAmbientMusic();
        GameObject.Destroy(stateMachine.Target);
        stateMachine.gameObject.GetComponent<CharacterController>().enabled = false;
        stateMachine.DestroyCharacterInTime(30f);
        
    }

    private IEnumerator WaitForAnimationToEnd()
    {
         
        yield return new WaitForSeconds(2.2f);

        // Instanciar y destruir los objetos correspondientes
        stateMachine.DestroyCharacter();
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
