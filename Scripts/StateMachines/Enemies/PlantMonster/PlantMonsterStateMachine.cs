using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;
using GameDevTV.Utils;
using RPG.Combat;

public class PlantMonsterStateMachine : StateMachine
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
    [field: SerializeField] public ThirdPersonWeaponConfig defaultWeapon {get; private set;}

    //Variables para el movimiento y el ataque
    [field: SerializeField] public float MovementSpeed{get; private set;} 
    [field: SerializeField] public float AttackRange{get; private set;} 
    [field: SerializeField] public float PlayerChasingRange{get; private set;} 
    [field: SerializeField] public float AttackKnockback{get; private set;} 
    [field: SerializeField] public bool isOnlySpellMagic{get; private set;} 

    //Variables para la magia
    [field: SerializeField] public GameObject puffParticle;
    [field: SerializeField] public ParticleSystem[] gravityParticle;

    //Variables para el patrullaje
    [field: SerializeField] public float ChaseDistance = 8f;

    [field: SerializeField] Transform rightHandTransform = null;
    [field: SerializeField] Transform leftHandTransform = null;

    public Health PlayerHealth {get; private set;} 
    ThirdPersonWeaponConfig currentWeaponConfig;
    LazyValue<ThirdPersonWeapon> currentWeapon;
    
    private BaseStats PlantMonsterBaseStats;
    private AudioController plantMonsterAudioController;

    private void Awake() 
    {
        currentWeaponConfig = defaultWeapon;
        currentWeapon = new LazyValue<ThirdPersonWeapon>(SetUpDefaultWeapon);
    }
    private ThirdPersonWeapon SetUpDefaultWeapon()
    {
        return AttachWeapon(defaultWeapon);
    }

    private ThirdPersonWeapon AttachWeapon(ThirdPersonWeaponConfig weapon)
    {
        Animator animator = GetComponent<Animator>();
        return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }

    private void Start()
    {
        PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        PlantMonsterBaseStats = GetComponent<BaseStats>(); 
        plantMonsterAudioController = GetComponent<AudioController>();

        if(Agent != null){
            Agent.updatePosition = false;
            Agent.updateRotation = false;
        }
        SwitchState(new PlantMonsterIdleState(this));
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
        SwitchState(new PlantMonsterImpactState(this));
    }

     private void HandleDie()
    {
        SwitchState(new PlantMonsterDeadState(this));
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
    public Health GetWarriorPlayerHealth()
    {
       return GameObject.FindWithTag("Player").GetComponent<Health>();
    }

    public float GetDamageStat(){
        return PlantMonsterBaseStats.GetStat(Stat.Damage);
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
    
    public void DesactiveAllPlantMonsterWeapon()
    {
        Weapon.gameObject.SetActive(false);
    }

    public void StopAllCourritines()
    {
        StopAllCoroutines();
    }

    public void PlayGetHitEffect()
    {
        EffectsToPlay.PlayGetHitEffect();
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
        plantMonsterAudioController.SetIsMonsterAttacking(newValue);
    }

//Unity animator event
    public void PlayPlantMonsterMoveEvent(){
        EventsToPlay.onMove?.Invoke();
    }
//Unity animator event
    public void PlayPlantMonsterAttackEvent(){
        EventsToPlay.onAttack?.Invoke();
    }
//Unity animator event
    public void IceBall(){
	    if(Health == null){return;}
        if(currentWeaponConfig == null){ return;}
        if(currentWeapon.value != null)
        {
            currentWeapon.value.OnHit();
        }

        float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
        if(currentWeaponConfig.HasProjectile())
        {
            currentWeaponConfig.LaunchProjectile(rightHandTransform, leftHandTransform,GetWarriorPlayerHealth(),gameObject, damage);
            return;
        }
        Health.TakeDamage(gameObject, damage,false, true);
	}
//Unity animator event
    public void CastingWarmup(int start){
		for (int i = 0; i < gravityParticle.Length; i++){
			if (start == 1){
				gravityParticle[i].Play();
			}else{
				gravityParticle[i].Stop();
			}
		}
	}
//Unity animator event
    public void Puff(){
        if(puffParticle == null){return;}
		GameObject newPuff = Instantiate (puffParticle, Health.transform.position, Quaternion.identity);
		Destroy (newPuff, 5.0f);
	}
}
