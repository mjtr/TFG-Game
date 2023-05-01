using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class VelociraptorStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator{get; private set;} 
    [field: SerializeField] public CharacterController Controller{get; private set;} 
    [field: SerializeField] public ForceReceived ForceReceived{get; private set;} 
    [field: SerializeField] public NavMeshAgent Agent{get; private set;} 
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
    public bool isSleeping = false;
    public bool isEating = false;
    public bool isDetectedPlayed = false;
    private bool firstTimeSeePlayer = true;
    private BaseStats VelociraptorBaseStats;
    private bool isWakingUp = false;
    private bool isFirstTimeToCallAllies = false;
    private bool hasHalfLife = false;
    private AudioController velociraptorAudioController;

    private void Start()
    {
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        VelociraptorBaseStats = GetComponent<BaseStats>(); 
        velociraptorAudioController = GetComponent<AudioController>();

        if(Agent != null){
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }  

        if(isSleeping)
        {
            SwitchState(new VelociraptorSleepState(this));
            return;
        }  

        if(isEating)
        {
            SwitchState(new VelociraptorEatingState(this));
            return;
        }

        if(PatrolPath != null) 
        { 
            SwitchState(new VelociraptorPatrolPathState(this));
            return;
        }

        SwitchState(new VelociraptorIdleState(this));
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
        if(!isSleeping && !GetWakeUp() && MustProduceGetHitAnimation())
        {
            SwitchState(new VelociraptorImpactState(this));
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
        SwitchState(new VelociraptorDeadState(this));
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

    public void SetWakeUp(bool newValue)
    {
        isWakingUp = newValue;
    }

    public bool GetWakeUp()
    {
        return isWakingUp;
    }

    public void SetIsFirstTimeToCallAllies(bool newValue)
    {
        isFirstTimeToCallAllies = newValue;
    }

    public bool GetIsFirstTimeToCallAllies()
    {
        return isFirstTimeToCallAllies;
    }

    public void SetHasHalfLife(bool newValue)
    {
        hasHalfLife = newValue;
    }

    public bool GetHasHalfLife()
    {
        return hasHalfLife;
    }


    public void EnableArmsDamage()
    {
        HeadDamage.SetAttack(GetDamageStat(), AttackKnockback);
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
        return VelociraptorBaseStats.GetStat(Stat.Damage);
    }

    public void SetChasingRange(float newChasingRange)
    {
        PlayerChasingRange = newChasingRange;
    }
    
    public void PlayGetHitEffect()
    {
        EffectsToPlay.PlayGetHitEffect();
    }

    public void DesactiveAllVelociraptorWeapon()
    {
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
        velociraptorAudioController.SetIsMonsterAttacking(newValue);
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
//Unity animator event
    public void AttackEvent(){
        EventsToPlay.onAttack?.Invoke();
    }

     public void Attack2Event(){
        EventsToPlay.onTailAttack?.Invoke();
    }
    
    public void ShoutEvent(){
        EventsToPlay.Shout?.Invoke();
    }
}
