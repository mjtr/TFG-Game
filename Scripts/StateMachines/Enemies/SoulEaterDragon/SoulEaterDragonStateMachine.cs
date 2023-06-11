using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class SoulEaterDragonStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator{get; private set;} 
    [field: SerializeField] public CharacterController Controller{get; private set;} 
    [field: SerializeField] public ForceReceived ForceReceived{get; private set;} 
    [field: SerializeField] public NavMeshAgent Agent{get; private set;} 
    [field: SerializeField] public WeaponDamage WeaponHead{get; private set;} 
    [field: SerializeField] public WeaponDamage WeaponTail{get; private set;} 
    [field: SerializeField] public Health Health{get; private set;} 
    [field: SerializeField] public EventsToPlay EventsToPlay{get; private set;} 
    [field: SerializeField] public EffectsToPlay EffectsToPlay{get; private set;} 
    [field: SerializeField] public Target Target{get; private set;} 
    [field: SerializeField] public RPG.Control.PatrolPath PatrolPath;
    [field: SerializeField] public SoulEaterDragonFirebreath SoulEaterDragonFirebreath{get; private set;} 
    [field: SerializeField] public GameObject SoulEaterDeathBody;


    //Variables para el movimiento y el ataque
    [field: SerializeField] public float MovementSpeed{get; private set;} 
    [field: SerializeField] public float AttackRange{get; private set;} 
    [field: SerializeField] public float PlayerChasingRange{get; private set;} 
    [field: SerializeField] public float AttackKnockback{get; private set;} 
    [field: SerializeField] public float MaxMagicRange{get; private set;} 
    [field: SerializeField] public float MinMagicRange{get; private set;} 


     //Variables para el patrullaje
    [field: SerializeField] public float ChaseDistance = 8f;
    [field: SerializeField] public float SuspiciousTime = 5f;
    [field: SerializeField] public float WaypointTolerance = 1f;
    [field: SerializeField] public float WaypointDwellTime = 3f;
    [field: SerializeField] public float MaxSpeed = 5f;
    [field:SerializeField] public float PatrolSpeedFraction = 0.8f;
    [field:SerializeField] public bool canThrowFireBall = true;

    public Health PlayerHealth {get; private set;} 

    public bool isDetectedPlayed = false; 
    private AudioController soulEaterDragonAudioController;

    private BaseStats SoulEaterDragonBaseStats;
    private bool isFlying = false;
    private float timeToFly = 0;
    private float timeToLand = 0;

    private void Start()
    {
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        SoulEaterDragonBaseStats = GetComponent<BaseStats>();
        soulEaterDragonAudioController = GetComponent<AudioController>(); 

        if(Agent != null){
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }
        if(PatrolPath != null) { SwitchState(new SoulEaterDragonPatrolPathState(this));}
        else{ SwitchState(new SoulEaterDragonIdleState(this));}
       
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
        if(!GetIsFlying() && MustProduceGetHitAnimation())
        {
            SwitchState(new SoulEaterDragonImpactState(this));
        }
        
    }
    public bool GetIsFlying(){
        return this.isFlying;
    }

    public void SetIsFlying(bool newValue){
        this.isFlying = newValue;
    }
    private bool MustProduceGetHitAnimation()
    {
        int num = Random.Range(0,20);
        if(num <= 8 ){
            return false;
        }     
        return true;
    }

     private void HandleDie()
    {
        SwitchState(new SoulEaterDragonDeadState(this));
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

    public WarriorPlayerStateMachine GetWarriorPlayerStateMachine()
    {
       return GameObject.FindWithTag("Player").GetComponent<WarriorPlayerStateMachine>();
    }

    public float GetFlyTime(){
        return timeToFly;
    }

    public void AddTimeToFlyTime(float time){
        timeToFly += time;
    }

    public void ResetFlyTime(){
        timeToFly = 0;
    }

    public float GetLandTime(){
        return timeToLand;
    }

    public void AddTimeToLandTime(float time){
        timeToLand += time;
    }

    public void ResetLandTime(){
        timeToLand = 0;
    }

    public void ResetNavhMesh()
    {
        Agent.enabled = true;
        Agent.ResetPath();
        Agent.enabled = true;
    }


    public EventsToPlay GetWarriorPlayerEvents()
    {
       return GameObject.FindWithTag("Player").GetComponent<EventsToPlay>();
    }

    public float GetDamageStat(){
        return SoulEaterDragonBaseStats.GetStat(Stat.Damage);
    }

    public void SetChasingRange(float newChasingRange)
    {
        PlayerChasingRange = newChasingRange;
    }
    
    public void PlayGetHitEffect()
    {
        EffectsToPlay.PlayGetHitEffect();
    }

    public void DesactiveAllSoulEaterDragonWeapon()
    {
        WeaponHead.gameObject.SetActive(false);
        WeaponTail.gameObject.SetActive(false);
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
    
    private bool IsPlayerNear()
    {
        if(PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (PlayerHealth.transform.position - transform.position).sqrMagnitude;

        return playerDistanceSqr <= (PlayerChasingRange + 20) * (PlayerChasingRange + 20);
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

    public void DestroyCharacter()
    {
        Destroy(gameObject);
    }
    
    public void InstanciateSoulEaterDeathBody(){
        if(SoulEaterDeathBody == null){return;}
        Instantiate(SoulEaterDeathBody, transform.position, transform.rotation);
    }

    public void SetAudioControllerIsAttacking(bool newValue)
    {
        soulEaterDragonAudioController.SetIsMonsterAttacking(newValue);
    }

//Unity animator event
    public void PlaySoulEaterDragonMoveEvent(){
        if(!IsPlayerNear()){return;}
        EventsToPlay.onMove?.Invoke();
    }
//Unity animator event
    public void PlaySoulEaterDragonAttackEvent(){
        EventsToPlay.onAttack?.Invoke();
    }
    public void PlaySoulEaterDragonTailAttackEvent(){
        EventsToPlay.onTailAttack?.Invoke();
    }

     public void PlaySoulEaterGetHitEvent(){
        EventsToPlay.GetHit?.Invoke();
    }
    
}
