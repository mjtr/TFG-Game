using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class DevilStateMachine : StateMachine
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
    [field: SerializeField] public float FireBallAttackRange{get; private set;} 
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
    
    private BaseStats DevilBaseStats;
    private bool isActionMusicStart = false;

    private void Start()
    {
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        DevilBaseStats = GetComponent<BaseStats>(); 

        if(Agent != null){
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }

        if(PatrolPath != null) 
        { 
            SwitchState(new DevilPatrolPathState(this));
        }
        else{ SwitchState(new DevilIdleState(this));}
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
        SwitchState(new DevilImpactState(this));
    }

     private void HandleDie()
    {
        SwitchState(new DevilDeadState(this));
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
        return DevilBaseStats.GetStat(Stat.Damage);
    }

    public void SetChasingRange(float newChasingRange)
    {
        PlayerChasingRange = newChasingRange;
    }
    
    public void PlayGetHitEffect()
    {
        EffectsToPlay.PlayGetHitEffect();
    }

    public void DesactiveAllDevilWeapon()
    {
        LeftArmsDamage.gameObject.SetActive(false);
        RightArmsDamage.gameObject.SetActive(false);
        HeadDamage.gameObject.SetActive(false);
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

    public void StopSounds()
    {
        gameObject.GetComponent<SFB_AudioManager>().StopLoop();
    }
    
    public bool GetIsActionMusicStart()
    {
        return isActionMusicStart;
    }

    public void SetIsActionMusicStart(bool newValue)
    {
        isActionMusicStart = newValue;
    }

    public void StartActionMusic() 
    {
        GetWarriorPlayerStateMachine().StopAmbientMusic();
        SetIsActionMusicStart(true);
        GetWarriorPlayerStateMachine().StartActionMusic();
    }
    public void StartAmbientMusic()
    {
        GetWarriorPlayerStateMachine().StopActionMusic();
        SetIsActionMusicStart(false);
        GetWarriorPlayerStateMachine().StartAmbientMusic();
    }

//Unity animator event
    public void MoveEvent(){
        EventsToPlay.onMove?.Invoke();
    }
//Unity animator event
    public void AttackEvent(){
        EventsToPlay.onAttack?.Invoke();
    }
    
}
