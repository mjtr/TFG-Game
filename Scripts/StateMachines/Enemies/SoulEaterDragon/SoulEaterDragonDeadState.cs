using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEaterDragonDeadState : SoulEaterDragonBaseState
{
    private readonly int SoulEaterDragonDeadHash = Animator.StringToHash("Die");

    private const float CrossFadeDuration = 0.1f;
    public SoulEaterDragonDeadState(SoulEaterDragonStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllSoulEaterDragonWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(SoulEaterDragonDeadHash, CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        stateMachine.StartAmbientMusic();
        GameObject.Destroy(stateMachine.Target);
        stateMachine.GetComponent<CharacterController>().enabled = false;          

        if(stateMachine.SoulEaterDeathBody != null)
        {
            stateMachine.StartCoroutine(WaitForAnimationToEnd());
        }
       
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        yield return new WaitForSeconds(2);
        // Instanciar y destruir los objetos correspondientes
        stateMachine.InstanciateSoulEaterDeathBody();
        stateMachine.DestroyCharacter();
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
