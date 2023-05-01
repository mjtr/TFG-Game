using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMonsterDeadState : CrabMonsterBaseState
{
    private readonly int CrabMonsterDeadHash = Animator.StringToHash("Die");
    private const float CrossFadeDuration = 0.1f;
    public CrabMonsterDeadState(CrabMonsterStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllCrabMonsterWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(CrabMonsterDeadHash, CrossFadeDuration);
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        stateMachine.gameObject.GetComponent<CharacterController>().enabled = false;
        GameObject.Destroy(stateMachine.Target);
        EndGame endGame = stateMachine.GetComponent<EndGame>();
        if(endGame != null)
        {
            stateMachine.StartCoroutine(EndActualGame(endGame));
        }
        else
        {
            stateMachine.DestroyCharacter(20f);
        }
    }

    IEnumerator EndActualGame(EndGame endGame)
    {
        yield return new WaitForSeconds(6);
        endGame.EndActualGame();
    }

    public override void Tick(float deltaTime){}

    public override void Exit(){ }
}
