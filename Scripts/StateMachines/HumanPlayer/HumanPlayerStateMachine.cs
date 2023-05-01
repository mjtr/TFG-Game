using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using UnityEngine;

public class HumanPlayerStateMachine : StateMachine
{
    //Le estamos diciendo que puede coger el InputReader desde el editor de Unity pero que solo de manera privada se podrá modifcar el valor
    [field: SerializeField] public InputReader InputReader{get; private set;} 
    [field: SerializeField] public CharacterController Controller{get; private set;} 
    [field: SerializeField] public Animator Animator{get; private set;} 
    [field: SerializeField] public Targeter Targeter{get; private set;} 
    [field: SerializeField] public ForceReceived ForceReceived{get; private set;} 
    [field: SerializeField] public WeaponDamage Weapon{get; private set;} 
    [field: SerializeField] public Health Health{get; private set;} 
    [field: SerializeField] public Ragdoll Ragdoll{get; private set;} 
    [field: SerializeField] public LedgeDetector LedgeDetector{get; private set;} 


    [field: SerializeField] public float FreeLookMovementSpeed{get; private set;} 
    [field: SerializeField] public float TargetingMovementSpeed{get; private set;} 
    [field: SerializeField] public float RotationDamping{get; private set;} 
    [field: SerializeField] public float DodgeLength {get; private set;} 
    [field: SerializeField] public float DodgeDuration {get; private set;} 
    [field: SerializeField] public float RollDuration {get; private set;} 
    [field: SerializeField] public float RollLength {get; private set;} 

    [field: SerializeField] public float JumpForce {get; private set;} 

    [field: SerializeField] public Attack[] Attacks{get; private set;} 

    public Transform MainCameraTransform{get; private set;} 

    private BaseStats mainCharacterBaseStats;

    private void Start(){
        mainCharacterBaseStats = GetComponent<BaseStats>(); 
        MainCameraTransform = Camera.main.transform;
        
        SwitchState(new HumanPlayerFreeLookState(this));
    }

    private void OnEnable()
    {
        Health.OnTakeDamageForInvokeImpactState += HandleTakeDamage;
        Health.OnDieForInvokeDeadState += HandleDie;
    }

    private void OnDisable()
    {
        Health.OnTakeDamageForInvokeImpactState -= HandleTakeDamage;
        Health.OnDieForInvokeDeadState -= HandleDie;
    }

    private void HandleTakeDamage()
    {
        SwitchState(new HumanPlayerImpactState(this));
    }

    private void HandleDie()
    {
        SwitchState(new HumanPlayerDeadState(this));
    }

    public float GetDamageStat(){
        return mainCharacterBaseStats.GetStat(Stat.Damage);
    }

}
