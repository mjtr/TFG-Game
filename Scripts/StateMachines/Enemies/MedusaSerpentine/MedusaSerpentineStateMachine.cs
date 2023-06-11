using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class MedusaSerpentineStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator{get; private set;} 
    [field: SerializeField] public CharacterController Controller{get; private set;} 
    [field: SerializeField] public ForceReceived ForceReceived{get; private set;} 
    [field: SerializeField] public NavMeshAgent Agent{get; private set;} 
    [field: SerializeField] public WeaponDamage WeaponSwordDamage{get; private set;} 
    [field: SerializeField] public WeaponDamage TailDamage{get; private set;} 
    [field: SerializeField] public WeaponDamage FinishTailDamage{get; private set;} 

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
    private float timeToScream = 0;
    
    private BaseStats MedusaSerpentineBaseStats;
    private AudioController MedusaSerpentineAudioController;

    private void Start()
    {
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        MedusaSerpentineBaseStats = GetComponent<BaseStats>(); 
        MedusaSerpentineAudioController = gameObject.GetComponent<AudioController>();

        if(Agent != null){
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }
        if(PatrolPath != null) 
        { 
            SwitchState(new MedusaSerpentinePatrolPathState(this));
        }
        else{ SwitchState(new MedusaSerpentineIdleState(this));}
       
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
        SetAudioControllerIsAttacking(true);
        StartActionMusic();
        if(MustProduceGetHitAnimation())
        {   
            StopParticlesEffects();
            DesactiveAllMedusaSerpentineWeapon();
            StopAllCoroutines();
            SwitchState(new MedusaSerpentineImpactState(this));
        }
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
        SwitchState(new MedusaSerpentineDeadState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
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
        return MedusaSerpentineBaseStats.GetStat(Stat.Damage);
    }

    public void SetChasingRange(float newChasingRange)
    {
        PlayerChasingRange = newChasingRange;
    }
    
    public void PlayGetHitEffect()
    {
        EffectsToPlay.PlayGetHitEffect();
    }

    public void DesactiveAllMedusaSerpentineWeapon()
    {
        WeaponSwordDamage.gameObject.SetActive(false);
        TailDamage.gameObject.SetActive(false);
        FinishTailDamage.gameObject.SetActive(false);
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

    public void DestroyCharacter()
    {
        Destroy(gameObject);
    }

    public void DestroyCharacterInTime(float time)
    {
        Destroy(gameObject, time);
    }
    private bool IsPlayerNear()
    {
        if(PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (PlayerHealth.transform.position - transform.position).sqrMagnitude;

        return playerDistanceSqr <= (PlayerChasingRange + 25) * (PlayerChasingRange + 25);
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

    public void SetAudioControllerIsAttacking(bool newValue)
    {
        MedusaSerpentineAudioController.SetIsMonsterAttacking(newValue);
    }
    
    public void ActiveMedusaSerpentineWeapon(){
        WeaponSwordDamage.gameObject.SetActive(true);
    }

    public void ActiveMedusaSerpentineTailWeapon(){
        TailDamage.gameObject.SetActive(true);
        FinishTailDamage.gameObject.SetActive(true);
    }


    public void ResetNavMesh()
    {
        Agent.enabled = true;
        Agent.ResetPath();
        Agent.enabled = false;
        Agent.enabled = true;
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

//Unity animator event
    public void MoveEvent(){
        if(!IsPlayerNear()){return;}
        EventsToPlay.onMove?.Invoke();
    }
//Unity animator event
    public void AttackEvent(){
        int num = Random.Range(0,3);
        if(num == 0){EventsToPlay.onAttack?.Invoke(); return;}
        if(num == 1){EventsToPlay.OnAttack2?.Invoke(); return;}
        EventsToPlay.OnAttack3?.Invoke();
    }
    
}
