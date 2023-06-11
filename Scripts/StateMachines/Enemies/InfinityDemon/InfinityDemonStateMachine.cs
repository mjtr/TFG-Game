using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class InfinityDemonStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator{get; private set;} 
    [field: SerializeField] public CharacterController Controller{get; private set;} 
    [field: SerializeField] public ForceReceived ForceReceived{get; private set;} 
    [field: SerializeField] public NavMeshAgent Agent{get; private set;} 
    [field: SerializeField] public WeaponDamage WeaponAxeDamage{get; private set;} 
    [field: SerializeField] public WeaponDamage RightFootDamage{get; private set;} 
    [field: SerializeField] public WeaponDamage LeftFootDamage{get; private set;} 

    [field: SerializeField] public Health Health{get; private set;} 
    [field: SerializeField] public EventsToPlay EventsToPlay{get; private set;} 
    [field: SerializeField] public EffectsToPlay EffectsToPlay{get; private set;} 
    [field: SerializeField] public Target Target{get; private set;} 
    [field: SerializeField] public RPG.Control.PatrolPath PatrolPath;
    [field: SerializeField] public GameObject InfinityDemonDeathBody;

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
    
    private BaseStats InfinityDemonBaseStats;
    private AudioController InfinityDemonAudioController;

    private void Start()
    {
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        InfinityDemonBaseStats = GetComponent<BaseStats>(); 
        InfinityDemonAudioController = gameObject.GetComponent<AudioController>();

        if(Agent != null){
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }
        if(PatrolPath != null) 
        { 
            SwitchState(new InfinityDemonPatrolPathState(this));
        }
        else{ SwitchState(new InfinityDemonIdleState(this));}
       
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
            DesactiveAllInfinityDemonWeapon();
            StopAllCoroutines();
            SwitchState(new InfinityDemonImpactState(this));
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
        SwitchState(new InfinityDemonDeadState(this));
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
        return InfinityDemonBaseStats.GetStat(Stat.Damage);
    }

    public void SetChasingRange(float newChasingRange)
    {
        PlayerChasingRange = newChasingRange;
    }
    
    public void PlayGetHitEffect()
    {
        EffectsToPlay.PlayGetHitEffect();
    }

    public void DesactiveAllInfinityDemonWeapon()
    {
        WeaponAxeDamage.gameObject.SetActive(false);
        RightFootDamage.gameObject.SetActive(false);
        LeftFootDamage.gameObject.SetActive(false);
    }

    public void ActiveAxeInfinityDemonWeapon()
    {
        WeaponAxeDamage.gameObject.SetActive(true);
    }

    public void ActiveRightFootInfinityDemonWeapon()
    {
        RightFootDamage.gameObject.SetActive(true);
    }

    public void ActiveLeftFootInfinityDemonWeapon()
    {
        LeftFootDamage.gameObject.SetActive(true);
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

    public void InstanciateInfinityDemonDeathBody(){
        if(InfinityDemonDeathBody == null){return;}
        Instantiate(InfinityDemonDeathBody, transform.position, transform.rotation);
    }

    private bool IsPlayerNear()
    {
        if(PlayerHealth.CheckIsDead()){return false;}

        float playerDistanceSqr = (PlayerHealth.transform.position - transform.position).sqrMagnitude;

        return playerDistanceSqr <= (PlayerChasingRange + 25) * (PlayerChasingRange + 25);
    }

    public void SetAudioControllerIsAttacking(bool newValue)
    {
        InfinityDemonAudioController.SetIsMonsterAttacking(newValue);
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
