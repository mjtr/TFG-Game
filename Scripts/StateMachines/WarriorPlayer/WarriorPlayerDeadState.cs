using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorPlayerDeadState : WarriorPlayerBaseState
{

    private readonly int PlayerDeadHash = Animator.StringToHash("Warrior_Death1");

    private const float CrossFadeDuration = 0.1f;

    public WarriorPlayerDeadState(WarriorPlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        //stateMachine.Ragdoll.ToggleRagdoll(true);
        stateMachine.GetWeaponDamage().gameObject.SetActive(false);
        stateMachine.Animator.CrossFadeInFixedTime(PlayerDeadHash, CrossFadeDuration);
        EndGame endGame = stateMachine.GetComponent<EndGame>();
        stateMachine.StartCoroutine(EndActualGame(endGame));
    }

    IEnumerator EndActualGame(EndGame endGame)
    {
        yield return new WaitForSeconds(2.5f);
        endGame.DeadCharacterEndGame();
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {

    }
}
