using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class DragonUsurperStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator{get; private set;} 
    [field: SerializeField] public CharacterController Controller{get; private set;} 
    [field: SerializeField] public ForceReceived ForceReceived{get; private set;} 
    [field: SerializeField] public NavMeshAgent Agent{get; private set;} 
    [field: SerializeField] public WeaponDamage HeadDamage{get; private set;} 
    [field: SerializeField] public WeaponDamage ArmRightDamage{get; private set;} 
    [field: SerializeField] public WeaponDamage ArmLeftDamage{get; private set;} 
    [field: SerializeField] public Health Health{get; private set;} 
    [field: SerializeField] public EventsToPlay EventsToPlay{get; private set;} 
    [field: SerializeField] public EffectsToPlay EffectsToPlay{get; private set;} 
    [field: SerializeField] public Target Target{get; private set;} 
    [field: SerializeField] public RPG.Control.PatrolPath PatrolPath;
    [field: SerializeField] public DragonUsurperFireBreath DragonUsurperFireBreath{get; private set;} 
    
	[field: SerializeField] private GameObject ClawEffect = null;
    [field: SerializeField] private GameObject PlaceToPlayClawEffect = null; 

    //Variables para el movimiento y el ataque
    [field: SerializeField] public float MovementSpeed{get; private set;} 
    [field: SerializeField] public float AttackRange{get; private set;} 
    [field: SerializeField] public float ClawAttackRange{get; private set;} 
    [field: SerializeField] public float MaxFireBreathAttackRange{get; private set;} 
    [field: SerializeField] public float MinFireBreathAttackRange{get; private set;} 
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
    private BaseStats DragonUsurperBaseStats;
    private AudioController dragonUsurperDragonController;

    private void Start()
    {
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        DragonUsurperBaseStats = GetComponent<BaseStats>(); 
        dragonUsurperDragonController = gameObject.GetComponent<AudioController>();
        if(Agent != null){
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }

        SwitchState(new DragonUsurperIdleState(this));
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
            SwitchState(new DragonUsurperImpactState(this));
        }
    }

     private void HandleDie()
    {
        SwitchState(new DragonUsurperDeadState(this));
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
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, ClawAttackRange);
        
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, MaxFireBreathAttackRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, MinFireBreathAttackRange);
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
        return DragonUsurperBaseStats.GetStat(Stat.Damage);
    }

    public void SetChasingRange(float newChasingRange)
    {
        PlayerChasingRange = newChasingRange;
    }
    
    public void PlayGetHitEffect()
    {
        EffectsToPlay.PlayGetHitEffect();
    }

    public void DesactiveAllDragonUsurperWeapon()
    {
        HeadDamage.gameObject.SetActive(false);
        ArmRightDamage.gameObject.SetActive(false);
        ArmLeftDamage.gameObject.SetActive(false);
        DragonUsurperFireBreath.FireBreathWeaponLogic.SetActive(false);
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

    public void ResetNavhMesh()
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

    public void SetAudioControllerIsAttacking(bool newValue)
    {
        dragonUsurperDragonController.SetIsMonsterAttacking(newValue);
    }

//Unity animator event
    public void PlayDragonUsurperMoveEvent(){
        EventsToPlay.onMove?.Invoke();
    }

    //Unity animator event
    public void PlayDragonUsurperClawEffects(){
        Transform placeWhereStartEffect = PlaceToPlayClawEffect.transform;
        GameObject clawEffect = Instantiate(ClawEffect, placeWhereStartEffect);
        Destroy(clawEffect, 0.6f);
    }
//Unity animator event
    public void PlayDragonUsurperBasicAttackEvent(){
        EventsToPlay.onAttack?.Invoke();
    }
    
    public void PlayDragonUsurperClawAttackEvent(){
        EventsToPlay.onTailAttack?.Invoke();
    }
    
    public void PlayDragonShoutEvent(){
        EventsToPlay.Shout?.Invoke();
    }
    
    public void PlayDragonFireBreathEvent(){
        EventsToPlay.Breath?.Invoke();
    }
}
