using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class DragonStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator{get; private set;} 
    [field: SerializeField] public CharacterController Controller{get; private set;} 
    [field: SerializeField] public ForceReceived ForceReceived{get; private set;} 
    [field: SerializeField] public NavMeshAgent Agent{get; private set;} 
    [field: SerializeField] public WeaponDamage WeaponHead{get; private set;} 
    [field: SerializeField] public WeaponDamage WeaponBody{get; private set;} 
    [field: SerializeField] public WeaponDamage WeaponTail{get; private set;} 
    [field: SerializeField] public WeaponDamage WeaponFinishTail{get; private set;} 
    [field: SerializeField] public Health Health{get; private set;} 
    [field: SerializeField] public EventsToPlay EventsToPlay{get; private set;} 
    [field: SerializeField] public EffectsToPlay EffectsToPlay{get; private set;} 
    [field: SerializeField] public Target Target{get; private set;} 
    [field: SerializeField] public Ragdoll Ragdoll{get; private set;} 
    [field: SerializeField] public DragonFirebreath DragonFirebreath{get; private set;} 

    //Variables para el movimiento y el ataque
    [field: SerializeField] public float MovementSpeed{get; private set;} 
    [field: SerializeField] public float AttackRange{get; private set;} 
    [field: SerializeField] public float MaxFireBreathAttackRange{get; private set;} 
    [field: SerializeField] public float MinFireBreathAttackRange{get; private set;} 
    [field: SerializeField] public float PlayerChasingRange{get; private set;} 
    [field: SerializeField] public float AttackKnockback{get; private set;} 

    //Variables para el patrullaje
    [field: SerializeField] public float ChaseDistance = 8f;

    public Health PlayerHealth {get; private set;} 
    public bool isDetectedPlayed = false;
    private BaseStats DragonBaseStats;
    private bool isActionMusicStart = false;

    private void Start()
    {
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        DragonBaseStats = GetComponent<BaseStats>(); 

        if(Agent != null){
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }
        SwitchState(new DragonIdleState(this));
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
        isDetectedPlayed = true;
        GetWarriorPlayerEvents().WarriorOnAttack?.Invoke();
        PlayGetHitEffect();
        if(MustProduceGetHitAnimation())
        {
            SwitchState(new DragonImpactState(this)); 
        }
    }

    private bool MustProduceGetHitAnimation()
    {
        int num = Random.Range(0,20);
        if(num <= 14 ){
            return false;
        }     
        return true;
    }

     private void HandleDie()
    {
        SwitchState(new DragonDeadState(this));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, MaxFireBreathAttackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, MinFireBreathAttackRange);
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
        return DragonBaseStats.GetStat(Stat.Damage);
    }

    public void SetChasingRange(float newChasingRange)
    {
        PlayerChasingRange = newChasingRange;
    }
    
    public void PlayGetHitEffect()
    {
        EffectsToPlay.PlayGetHitEffect();
    }

    public void DesactiveAllDragonWeapon()
    {
        WeaponBody.gameObject.SetActive(false);
        WeaponHead.gameObject.SetActive(false);
        WeaponTail.gameObject.SetActive(false);
        WeaponFinishTail.gameObject.SetActive(false);
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

    public void StopDragonSounds()
    {
        gameObject.GetComponent<SFB_AudioManager>().StopLoop();
    }

//Unity animator event
    public void PlayDragonMoveEvent(){
        EventsToPlay.onMove?.Invoke();
    }
//Unity animator event
    public void PlayDragonAttackEvent(){
        EventsToPlay.onAttack?.Invoke();
    }
    
}
