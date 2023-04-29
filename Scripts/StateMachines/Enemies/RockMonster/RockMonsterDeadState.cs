using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMonsterDeadState : RockMonsterBaseState
{
    private readonly int RockMonsterDeadHash = Animator.StringToHash("Death");

    private const float CrossFadeDuration = 0.1f;
    public RockMonsterDeadState(RockMonsterStateMachine stateMachine) : base(stateMachine){ }

    public override void Enter()
    {
        stateMachine.GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        stateMachine.PlayGetHitEffect();
        stateMachine.StopSounds();
        stateMachine.StopAllCourritines();
        stateMachine.StopParticlesEffects();
        stateMachine.DesactiveAllRockMonsterWeapon();
        stateMachine.Animator.CrossFadeInFixedTime(RockMonsterDeadHash, CrossFadeDuration);
        stateMachine.StartAmbientMusic();
        stateMachine.GetWarriorPlayerStateMachine().Targeter.RemoveTarget(stateMachine.Target);
        GameObject.Destroy(stateMachine.Target);
        stateMachine.GetComponent<CharacterController>().enabled = false;
       
        EndGame endGame = stateMachine.GetComponent<EndGame>();

        if(endGame != null)
        {
            stateMachine.StartCoroutine(EndActualGame(endGame));
        }
        else
        {
            stateMachine.DestroyCharacter(8f);
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
