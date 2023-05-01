using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class MushroomStateMachine : StateMachine
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
    [field: SerializeField] public Ragdoll Ragdoll{get; private set;} 

    //Variables para el movimiento y el ataque
    [field: SerializeField] public float MovementSpeed{get; private set;} 
    [field: SerializeField] public float AttackRange{get; private set;} 
    [field: SerializeField] public float MaxMagicRange{get; private set;} 
    [field: SerializeField] public float MinMagicRange{get; private set;} 
    [field: SerializeField] public float PlayerChasingRange{get; private set;} 
    [field: SerializeField] public float AttackKnockback{get; private set;} 
    
    //Variables para el patrullaje
    [field: SerializeField] public float ChaseDistance = 8f;

    public Health PlayerHealth {get; private set;} 

    private BaseStats MushroomBaseStats;
    private bool isFirstTimeToSeePlayer = true;
    private AudioController mushroomAudioController;

    private void Start()
    {
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        MushroomBaseStats = GetComponent<BaseStats>(); 
        mushroomAudioController = GetComponent<AudioController>();

        Agent.updatePosition = false;
        Agent.updateRotation = false;

        SwitchState(new MushroomIdleState(this));
    }

    public void DesactiveAllDemonWeapon()
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

        GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        PlayGetHitEffect();
        
        if(MustProduceGetHitAnimation())
        {
            SwitchState(new MushroomImpactState(this));
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
        SwitchState(new MushroomDeadState(this));
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
        return MushroomBaseStats.GetStat(Stat.Damage);
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
        mushroomAudioController.SetIsMonsterAttacking(newValue);
    }

//Unity animator event
    public void PlayMushroomMoveEvent(){
        EventsToPlay.onMove?.Invoke();
    }
//Unity animator event
    public void PlayMushroomMove2Event(){
        EventsToPlay.onMove2?.Invoke();
    }
//Unity animator event
    public void PlayMushroomAttackEvent(){
        EventsToPlay.onAttack?.Invoke();
    }
    
}
