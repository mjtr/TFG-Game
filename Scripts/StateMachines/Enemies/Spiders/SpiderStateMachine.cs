using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class SpiderStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator{get; private set;} 
    [field: SerializeField] public CharacterController Controller{get; private set;} 
    [field: SerializeField] public ForceReceived ForceReceived{get; private set;} 
    [field: SerializeField] public NavMeshAgent Agent{get; private set;} 
    [field: SerializeField] public WeaponDamage Weapon{get; private set;} 
    [field: SerializeField] public Health Health{get; private set;} 
    [field: SerializeField] public EventsToPlay EventsToPlay{get; private set;} 
    [field: SerializeField] public EffectsToPlay EffectsToPlay{get; private set;} 
    [field: SerializeField] public Target Target{get; private set;} 
    [field: SerializeField] public RPG.Control.PatrolPath PatrolPath;


    //Variables para el movimiento y el ataque
    [field: SerializeField] public float MovementSpeed{get; private set;} 
    [field: SerializeField] public float AttackRange{get; private set;} 
    [field: SerializeField] public float MaxMagicRange{get; private set;} 
    [field: SerializeField] public float MinMagicRange{get; private set;} 
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
    private BaseStats SpiderBaseStats;
    private bool isFirstTimeToSeePlayer = true;
    private AudioController SpiderAudioController;
    private float timeToScream = 0;


    private void Start()
    {
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        SpiderBaseStats = GetComponent<BaseStats>(); 
        SpiderAudioController = GetComponent<AudioController>();

        Agent.updatePosition = false;
        Agent.updateRotation = false;

        if(PatrolPath != null) 
        { 
            SwitchState(new SpiderPatrolPathState(this));
        }
        else{ SwitchState(new SpiderIdleState(this));}
    }

    public void DesactiveAllSpiderWeapon()
    {
        Weapon.gameObject.SetActive(false);
    }

    public void StopParticlesEffects()
    {
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
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
        if(isFirstTimeToSeePlayer)
        {
            SetChasingRange(PlayerChasingRange + 6f);
            isFirstTimeToSeePlayer = false;
        }
        isDetectedPlayed = true;
        GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        PlayGetHitEffect();
        
        if(MustProduceGetHitAnimation())
        {
            SwitchState(new SpiderImpactState(this));
        }
    }

    public float GetScreamTime(){
        return timeToScream;
    }

    public void AddTimeToScreamTime(float time){
        timeToScream += time;
    }

    public void ResetScreamTime(){
        timeToScream = 0;
    }

    private bool MustProduceGetHitAnimation()
    {
        int num = Random.Range(0,20);
        if(num <= 6 ){
            return false;
        }     
        return true;
    }

     private void HandleDie()
    {
        SwitchState(new SpiderDeadState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, MaxMagicRange);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, MinMagicRange);
    }

    public void ResetNavMesh()
    {
        Agent.enabled = true;
        Agent.ResetPath();
        Agent.enabled = false;
        Agent.enabled = true;
    }

    public PlayerStateMachine GetPlayerStateMachine()
    {
       return GameObject.FindWithTag("Player").GetComponent<PlayerStateMachine>();
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
        return SpiderBaseStats.GetStat(Stat.Damage);
    }

    public void SetChasingRange(float newChasingRange)
    {
        PlayerChasingRange = newChasingRange;
    }
    
    public void PlayGetHitEffect()
    {
        EffectsToPlay.PlayGetHitEffect(); 
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
        SpiderAudioController.SetIsMonsterAttacking(newValue);
    }

    
    public void ActiveSpiderWeapon(){
        Weapon.gameObject.SetActive(true);
    }


//Unity animator event
    public void PlaySpiderMoveEvent(){
        EventsToPlay.onMove?.Invoke();
    }
//Unity animator event
    public void PlaySpiderMove2Event(){
        EventsToPlay.onMove2?.Invoke();
    }
//Unity animator event
    public void PlaySpiderAttackEvent(){
        EventsToPlay.onAttack?.Invoke();
    }
    
}
