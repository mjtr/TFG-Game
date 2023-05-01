using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class CrabMonsterStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator{get; private set;} 
    [field: SerializeField] public CharacterController Controller{get; private set;} 
    [field: SerializeField] public ForceReceived ForceReceived{get; private set;} 
    [field: SerializeField] public NavMeshAgent Agent{get; private set;} 
    [field: SerializeField] public WeaponDamage LeftArmsDamage{get; private set;} 
    [field: SerializeField] public WeaponDamage RightArmsDamage{get; private set;} 
    [field: SerializeField] public WeaponDamage HeadDamage{get; private set;} 
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
    private bool firstTimeSeePlayer = true;
    private float timeToUseHealMagic = 0;
    private float timeToUseExplosionMagic = 0;

    private bool firstTimeToPlayEpicMusic = true;

    private BaseStats CrabMonsterBaseStats;
    private AudioController crabMonsterAudioController;

    private void Start()
    {
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        CrabMonsterBaseStats = GetComponent<BaseStats>(); 
        crabMonsterAudioController = GetComponent<AudioController>();

        if(Agent != null){
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }

        if(PatrolPath != null) 
        { 
            SwitchState(new CrabMonsterPatrolPathState(this));
        }
        else{ SwitchState(new CrabMonsterIdleState(this));}
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
        if(MustProduceGetHitAnimation())
        {
            StopAllCourritines();
            SwitchState(new CrabMonsterImpactState(this));
        }
        
    }

    private bool MustProduceGetHitAnimation()
    {
        int num = Random.Range(0,20);
        if(num <= 16 ){
            return false;
        }     
        return true;
    }

     private void HandleDie()
    {
        SwitchState(new CrabMonsterDeadState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    public void SetFirsTimeToSeePlayer()
    {
        firstTimeSeePlayer = false;
    }

    public void EnableArmsDamage()
    {
        LeftArmsDamage.SetAttack(GetDamageStat(), AttackKnockback);
        RightArmsDamage.SetAttack(GetDamageStat(), AttackKnockback);
    }

    public void EnableArmsWeapon()
    {
        LeftArmsDamage.gameObject.SetActive(true);
        RightArmsDamage.gameObject.SetActive(true);
    }

    public void ResetNavhMesh()
    {
        Agent.enabled = false;
        Agent.enabled = true;
        Agent.ResetPath();
        Agent.enabled = false;
        Agent.enabled = true;
    }

    public bool GetFirsTimeTotSeePlayer()
    {
        return firstTimeSeePlayer;
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
        return CrabMonsterBaseStats.GetStat(Stat.Damage);
    }

    public void SetChasingRange(float newChasingRange)
    {
        PlayerChasingRange = newChasingRange;
    }
    
    public void PlayGetHitEffect()
    {
        EffectsToPlay.PlayGetHitEffect();
    }

    public void DesactiveAllCrabMonsterWeapon()
    {
        LeftArmsDamage.gameObject.SetActive(false);
        RightArmsDamage.gameObject.SetActive(false);
        HeadDamage.gameObject.SetActive(false);
    }

    public void StopAllCourritines()
    {
        StopAllCoroutines();
    }

    public void DestroyCharacter(float time)
    {
        Destroy(gameObject, time);
    }

    public void SetFirstTimeToPlayEpicMusic()
    {
        firstTimeToPlayEpicMusic = false;
    }

    public bool GetFirstTimeToPlayEpicMusic()
    {
        return firstTimeToPlayEpicMusic;
    }

    public void StartEpicMusic() 
    {
        GetWarriorPlayerStateMachine().StartEpicMusic();
    }

    public float GetExplosionTime(){
        return timeToUseExplosionMagic;
    }

    public void AddTimeToExplosionTime(float time){
        timeToUseExplosionMagic += time;
    }

    public void ResetExplosionTime(){
        timeToUseExplosionMagic = 0;
    }

    public float GetHealTime(){
        return timeToUseHealMagic;
    }

    public void AddTimeToHealTime(float time){
        timeToUseHealMagic += time;
    }

    public void ResetHealTime(){
        timeToUseHealMagic = 0;
    }

    public void StopParticlesEffects()
    {
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particles) {
            Destroy(ps);
        }
        Light[] lights = GetComponentsInChildren<Light>();
        foreach (Light ps in lights) {
            Destroy(ps);
        }
    }

    public void SetAudioControllerIsAttacking(bool newValue)
    {
        crabMonsterAudioController.SetIsMonsterAttacking(newValue);
    }

    private bool IsPlayerNear()
    {
        if(PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (PlayerHealth.transform.position - transform.position).sqrMagnitude;

        return playerDistanceSqr <= (PlayerChasingRange + 12) * (PlayerChasingRange + 12);
    }

//Unity animator event
    public void MoveEvent(){
        if(!IsPlayerNear()){return;}
        EventsToPlay.onMove?.Invoke();
    }

    public void Move2Event(){
        if(!IsPlayerNear()){return;}
        EventsToPlay.onMove2?.Invoke();
    }

//Unity animator event
    public void AttackEvent(){
        int num = Random.Range(0,3);
        if(num == 0){EventsToPlay.onAttack?.Invoke(); return;}
        if(num == 1){EventsToPlay.OnAttack2?.Invoke(); return;}
        EventsToPlay.OnAttack3?.Invoke();
    }
    
    public void ShoutEvent(){
        EventsToPlay.Shout?.Invoke();
    }
}
