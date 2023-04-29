using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;

public class GiantWormStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator{get; private set;} 
    [field: SerializeField] public CharacterController Controller{get; private set;} 
    [field: SerializeField] public ForceReceived ForceReceived{get; private set;} 
    [field: SerializeField] public NavMeshAgent Agent{get; private set;} 
    [field: SerializeField] public WeaponDamage Body{get; private set;} 
    [field: SerializeField] public WeaponDamage Head{get; private set;} 
    [field: SerializeField] public GameObject PoisonCollider; 
    [field: SerializeField] public GameObject GiantWormDeathBody;

    [field: SerializeField] public Health Health{get; private set;} 
    [field: SerializeField] public EventsToPlay EventsToPlay{get; private set;} 
    [field: SerializeField] public EffectsToPlay EffectsToPlay{get; private set;} 
     [field: SerializeField] public GameObject BoneToCalculateAttackRange{get; private set;} 

    [field: SerializeField] public Target Target{get; private set;} 
    [field: SerializeField] public ThirdPersonWeaponConfig defaultWeapon {get; private set;}

    //Variables para el movimiento y el ataque
    [field: SerializeField] public float MovementSpeed{get; private set;} 
    [field: SerializeField] public float AttackRange{get; private set;} 
    [field: SerializeField] public float PlayerChasingRange{get; private set;} 
    [field: SerializeField] public float AttackKnockback{get; private set;} 

    //Variables para el patrullaje
    [field: SerializeField] public float ChaseDistance = 8f;

    public Health PlayerHealth {get; private set;} 
   
    private BaseStats GiantWormBaseStats;
    private bool isActionMusicStart = false;

    private void Start()
    {
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        GiantWormBaseStats = GetComponent<BaseStats>(); 

        if(Agent != null){
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }
        SwitchState(new GiantWormHiddenState(this));
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
        if(MustProduceGetHitAnimation()){
            SwitchState(new GiantWormImpactState(this));
        }
    }

    private void HandleDie()
    {
        SwitchState(new GiantWormDeadState(this));
    }

    private bool MustProduceGetHitAnimation()
    {
        int num = Random.Range(0,20);
        if(num <= 17 ){
            return false;
        }     
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(BoneToCalculateAttackRange.transform.position, AttackRange);
    }
    
    public void StopAllCourritines()
    {
        StopAllCoroutines();
    }

    public WarriorPlayerStateMachine GetWarriorPlayerStateMachine()
    {
       return GameObject.FindWithTag("Player").GetComponent<WarriorPlayerStateMachine>();
    }

    public EventsToPlay GetWarriorPlayerEvents()
    {
       return GameObject.FindWithTag("Player").GetComponent<EventsToPlay>();
    }
    public Health GetWarriorPlayerHealth()
    {
       return GameObject.FindWithTag("Player").GetComponent<Health>();
    }

    public float GetDamageStat(){
        return GiantWormBaseStats.GetStat(Stat.Damage);
    }

    public void SetChasingRange(float newChasingRange)
    {
        PlayerChasingRange = newChasingRange;
    }

    public void StopParticlesEffects()
    {
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particles) {
            ps.Stop();
        }
    }
    public void DesactiveAllWormWeapon()
    {
        Body.gameObject.SetActive(false);
        Head.gameObject.SetActive(false);
    }
    
    public void StopWormSounds()
    {
        gameObject.GetComponent<SFB_AudioManager>().StopLoop();
    }
 
    public void PlayGetHitEffect()
    {
        EffectsToPlay.PlayGetHitEffect();
    }

    public void DestroyCharacter(float time)
    {
        Destroy(gameObject, time);
    }

     public void DestroyCharacter()
    {
        Destroy(gameObject);
    }
    
    public void InstanciateGiantWormDeathBody(){
        if(GiantWormDeathBody == null){return;}
        Instantiate(GiantWormDeathBody, transform.position, transform.rotation);
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
    public void PlayGiantWormMoveEvent(){
        EventsToPlay.onMove?.Invoke();
    }
    //Unity animator event
    public void PlayGiantWormAttackEvent(){
        EventsToPlay.onAttack?.Invoke();
    }

    //Unity animator event
    public void ActivePosionCollider(){
        PoisonCollider.SetActive(true);
    }
    //Unity animator event
    public void DesactivePosionCollider(){
        PoisonCollider.SetActive(false);
    }

}
