using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class RhinoStateMachine : StateMachine
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
    [field: SerializeField] public float PlayerChasingRange{get; private set;} 
    [field: SerializeField] public float AttackKnockback{get; private set;} 
    
    //Variables para el patrullaje
    [field: SerializeField] public float ChaseDistance = 8f;
    [field: SerializeField] public float SuspiciousTime = 5f;
    [field: SerializeField] public float WaypointTolerance = 1f;
    [field: SerializeField] public float WaypointDwellTime = 3f;
    [field: SerializeField] public float MaxSpeed = 5f;
    [field:SerializeField] public float PatrolSpeedFraction = 0.8f;
    [field:SerializeField] public bool isEating = true;
    
    public Health PlayerHealth {get; private set;} 
    public bool isDetectedPlayed = false;

    private BaseStats RhinoBaseStats;
    private bool isActionMusicStart = false;
    private bool firstTimeSeePlayer = true;
    private void Start()
    {
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        RhinoBaseStats = GetComponent<BaseStats>(); 

        if(Agent != null){
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }  

        if(isEating)
        {
            SwitchState(new RhinoEatingState(this));
            return;
        }

        if(PatrolPath != null) 
        { 
            SwitchState(new RhinoPatrolPathState(this));
            return;
        }

        SwitchState(new RhinoIdleState(this));
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
            SwitchState(new RhinoImpactState(this));
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
        SwitchState(new RhinoDeadState(this));
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

    public void ResetNavMesh()
    {
        Agent.enabled = true;
        Agent.ResetPath();
        Agent.enabled = false;
        Agent.enabled = true;
    }

     public void DesactiveAllRhinoWeapon()
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

    public float GetDamageStat(){
        return RhinoBaseStats.GetStat(Stat.Damage);
    }

    public bool GetFirsTimeTotSeePlayer()
    {
        return firstTimeSeePlayer;
    }

    public void SetFirsTimeToSeePlayer()
    {
        firstTimeSeePlayer = false;
    }

    public void SetPlayerChasingRange(float newRange)
    {
        PlayerChasingRange = newRange;
    }

    public void PlayGetHitEffect()
    {
        EffectsToPlay.PlayGetHitEffect();
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
        GetWarriorPlayerStateMachine().StartActionMusic2();
    }
    public void StartAmbientMusic()
    {
        GetWarriorPlayerStateMachine().StopActionMusic2();
        SetIsActionMusicStart(false);
        GetWarriorPlayerStateMachine().StartAmbientMusic();
    }

//Unity animator event
    public void PlayRhinoMoveEvent(){
        if(!IsPlayerNear()){return;}
        EventsToPlay.onMove?.Invoke();
    }

//Unity animator event
    public void PlayRhinoAttackEvent(){
        EventsToPlay.onAttack?.Invoke();
    }

    public void PlayRhinoShoutEvent(){
        EventsToPlay.Shout?.Invoke();
    }
    
}
