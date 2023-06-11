using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class TerrorDragonStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator{get; private set;} 
    [field: SerializeField] public CharacterController Controller{get; private set;} 
    [field: SerializeField] public ForceReceived ForceReceived{get; private set;} 
    [field: SerializeField] public NavMeshAgent Agent{get; private set;} 
    [field: SerializeField] public WeaponDamage WeaponHead{get; private set;} 
    [field: SerializeField] public WeaponDamage WeaponRightWing{get; private set;} 
    [field: SerializeField] public WeaponDamage WeaponLefttWing{get; private set;} 
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
    [field: SerializeField] public float MaxMagicRange{get; private set;} 
    [field: SerializeField] public float MinMagicRange{get; private set;} 
    [field: SerializeField] public float MaxFlyMagicRange{get; private set;} 


     //Variables para el patrullaje
    [field: SerializeField] public float ChaseDistance = 8f;
    [field: SerializeField] public float SuspiciousTime = 5f;
    [field: SerializeField] public float WaypointTolerance = 1f;
    [field: SerializeField] public float WaypointDwellTime = 3f;
    [field: SerializeField] public float MaxSpeed = 5f;
    [field:SerializeField] public float PatrolSpeedFraction = 0.8f;

    public Health PlayerHealth {get; private set;} 

    public bool isDetectedPlayed = false; 
    private AudioController TerrorDragonAudioController;

    private BaseStats TerrorDragonBaseStats;
    private bool isFlying = false;
    private float timeToFly = 0;
    private float timeToLand = 0;
    private float timeToScream = 0;

    private void Start()
    {
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        TerrorDragonBaseStats = GetComponent<BaseStats>();
        TerrorDragonAudioController = GetComponent<AudioController>(); 

        if(Agent != null){
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }
        if(PatrolPath != null) { SwitchState(new TerrorDragonPatrolPathState(this));}
        else{ SwitchState(new TerrorDragonIdleState(this));}
       
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
            SwitchState(new TerrorDragonImpactState(this));
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
        SwitchState(new TerrorDragonDeadState(this));
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, MaxFlyMagicRange);
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

    public float GetScreamTime(){
        return timeToScream;
    }

    public void AddTimeToScreamTime(float time){
        timeToScream += time;
    }

    public void ResetScreamTime(){
        timeToScream = 0;
    }

    public EventsToPlay GetWarriorPlayerEvents()
    {
       return GameObject.FindWithTag("Player").GetComponent<EventsToPlay>();
    }

    public float GetDamageStat(){
        return TerrorDragonBaseStats.GetStat(Stat.Damage);
    }

    public void SetChasingRange(float newChasingRange)
    {
        PlayerChasingRange = newChasingRange;
    }
    
    public void PlayGetHitEffect()
    {
        EffectsToPlay.PlayGetHitEffect();
    }

    public void DesactiveAllTerrorDragonWeapon()
    {
        WeaponHead.gameObject.SetActive(false);
        WeaponLefttWing.gameObject.SetActive(false);
        WeaponRightWing.gameObject.SetActive(false);
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

    public void SetAudioControllerIsAttacking(bool newValue)
    {
        TerrorDragonAudioController.SetIsMonsterAttacking(newValue);
    }

    public void ResetNavhMesh()
    {
        Agent.enabled = true;
        Agent.ResetPath();
        Agent.enabled = false;
        Agent.enabled = true;
    }


//Unity animator event
    public void PlayTerrorDragonMoveEvent(){
        if(!IsPlayerNear()){return;}
        EventsToPlay.onMove?.Invoke();
    }
//Unity animator event
    public void PlayTerrorDragonAttackEvent(){
        EventsToPlay.onAttack?.Invoke();
    }
    public void PlayTerrorDragonWingAttackEvent(){
        EventsToPlay.onTailAttack?.Invoke();
    }

     public void PlayTerrorGetHitEvent(){
        EventsToPlay.GetHit?.Invoke();
    }

    public void PlayTerrorScreamEvent(){
        EventsToPlay.Shout?.Invoke();
    }
    
    public void PlayAttack2Event(){
        EventsToPlay.OnAttack2?.Invoke();
    }

}
