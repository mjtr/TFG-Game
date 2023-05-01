using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class DragonNightmareStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator{get; private set;} 
    [field: SerializeField] public CharacterController Controller{get; private set;} 
    [field: SerializeField] public ForceReceived ForceReceived{get; private set;} 
    [field: SerializeField] public NavMeshAgent Agent{get; private set;} 
    [field: SerializeField] public WeaponDamage HeadDamage{get; private set;} 
    [field: SerializeField] public WeaponDamage ArmRightDamage{get; private set;} 
    [field: SerializeField] public Health Health{get; private set;} 
    [field: SerializeField] public EventsToPlay EventsToPlay{get; private set;} 
    [field: SerializeField] public EffectsToPlay EffectsToPlay{get; private set;} 
    [field: SerializeField] public Target Target{get; private set;} 
    [field: SerializeField] public RPG.Control.PatrolPath PatrolPath;

    //Variables para el movimiento y el ataque
    [field: SerializeField] public float MovementSpeed{get; private set;} 
    [field: SerializeField] public float AttackRange{get; private set;} 
    [field: SerializeField] public float PlayerChasingRange{get; private set;} 
    [field: SerializeField] public float AttackKnockback{get; private set;} 

     //Variables para el patrullaje
    [field: SerializeField] public float ChaseDistance = 8f;
    [field: SerializeField] public float SuspiciousTime = 5;
    [field: SerializeField] public float WaypointTolerance = 1f;
    [field: SerializeField] public float WaypointDwellTime = 3f;
    [field: SerializeField] public float MaxSpeed = 5f;
    [field:SerializeField] public float PatrolSpeedFraction = 0.8f;

    public Health PlayerHealth {get; private set;} 
    public bool isDetectedPlayed = false;
    private bool firstTimeToSeePlayer = true;
    private BaseStats DragonNightmareBaseStats;
    private AudioController dragonNightMareAudioController;

    private void Start()
    {
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        DragonNightmareBaseStats = GetComponent<BaseStats>(); 
        dragonNightMareAudioController = gameObject.GetComponent<AudioController>();
        if(Agent != null){
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }

        SwitchState(new DragonNightmareIdleState(this));
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
        GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        PlayGetHitEffect();
        isDetectedPlayed = true;
        SetFirstTimeToSeePlayer(false);
        if(MustProduceGetHitAnimation()){
            SwitchState(new DragonNightmareImpactState(this));
        }
    }

     private void HandleDie()
    {
        SwitchState(new DragonNightmareDeadState(this));
    }

     private bool MustProduceGetHitAnimation()
    {
        int num = Random.Range(0,20);
        if(num <= 16 ){
            return false;
        }     
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        
    }

    public bool GetFirstTimeToSeePlayer()
    {
        return firstTimeToSeePlayer;
    }

    public void SetFirstTimeToSeePlayer(bool newValue)
    {
        firstTimeToSeePlayer = newValue;
    }

    public WarriorPlayerStateMachine GetWarriorPlayerStateMachine()
    {
       return GameObject.FindWithTag("Player").GetComponent<WarriorPlayerStateMachine>();
    }

    public EventsToPlay GetWarriorPlayerEvents()
    {
       return GameObject.FindWithTag("Player").GetComponent<EventsToPlay>();
    }

    public float GetDamageStat(){
        return DragonNightmareBaseStats.GetStat(Stat.Damage);
    }

    public void SetChasingRange(float newChasingRange)
    {
        PlayerChasingRange = newChasingRange;
    }
    
    public void PlayGetHitEffect()
    {
        EffectsToPlay.PlayGetHitEffect();
    }

    public void DesactiveAllDragonNightmareWeapon()
    {
        HeadDamage.gameObject.SetActive(false);
        ArmRightDamage.gameObject.SetActive(false);
    }

    public void StopParticlesEffects()
    {
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        if(particles == null){return;}
        foreach (ParticleSystem ps in particles) {
            ps.Stop();
        }
    }
    public void StopAllCourritines()
    {
        StopAllCoroutines();
    }

    public void DestroyCharacter(float time)
    {
        Destroy(gameObject, time);
    }

    public void StartActionMusic() 
    {
        GetWarriorPlayerStateMachine().StopAmbientMusic();
        GetWarriorPlayerStateMachine().StartActionMusic();
    }
    public void StartAmbientMusic()
    {
        GetWarriorPlayerStateMachine().StopActionMusic();
        GetWarriorPlayerStateMachine().StartAmbientMusic();
    }

    public void SetAudioControllerIsAttacking(bool newValue)
    {
        dragonNightMareAudioController.SetIsMonsterAttacking(newValue);
    }

//Unity animator event
    public void PlayDragonNightmareMoveEvent(){
        EventsToPlay.onMove?.Invoke();
    }

//Unity animator event
    public void PlayDragonNightmareBasicAttackEvent(){
        EventsToPlay.onAttack?.Invoke();
    }

    public void PlayDragonNightmareClawnAttackEvent(){
        EventsToPlay.onTailAttack?.Invoke();
    }
    
    public void PlayDragonShoutEvent(){
        EventsToPlay.Shout?.Invoke();
    }
    

}
