using System;
using RPG.Combat;
using RPG.Stats;
using UnityEngine.AI;
using UnityEngine;

[Serializable]
public class WarriorPlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader{get; private set;} 
    [field: SerializeField] public CharacterController Controller{get; private set;} 
    [field: SerializeField] public Animator Animator{get; private set;} 
    [field: SerializeField] public Targeter Targeter{get; private set;} 
    [field: SerializeField] public ForceReceived ForceReceived{get; private set;} 
    [field: SerializeField] public Health Health{get; private set;} 
    [field: SerializeField] public EventsToPlay EventsToPlay{get; private set;} 
    [field: SerializeField] public EffectsToPlay EffectsToPlay{get; private set;} 
    [field: SerializeField] public Stamina Stamina{get; private set;} 
    [field: SerializeField] public Ragdoll Ragdoll{get; private set;} 
    [field: SerializeField] public LedgeDetector LedgeDetector{get; private set;} 
    [field: SerializeField] public ThirdPersonWeaponConfig DefaultWeapon {get; private set;}
    [field: SerializeField] public GameObject Shield {get; private set;}
    [SerializeField] public WeaponsLogicToPlay WeaponsLogicToPlay;

    [field: SerializeField] public float FreeLookMovementSpeed{get; private set;} 
    [field: SerializeField] public float SprintStaminaTaked {get; private set;} 
    [field: SerializeField] public float TargetingMovementSpeed{get; private set;} 
    [field: SerializeField] public float RotationDamping{get; private set;} 
    [field: SerializeField] public float DodgeLength {get; private set;} 
    [field: SerializeField] public float DodgeDuration {get; private set;} 
    [field: SerializeField] public float DodgeStaminaTaked {get; private set;} 
    [field: SerializeField] public float RollDuration {get; private set;} 
    [field: SerializeField] public float RollLength {get; private set;} 
    [field: SerializeField] public float JumpForce {get; private set;} 

    [field: SerializeField] Transform rightHandTransform = null;
    [field: SerializeField] Transform leftHandTransform = null;
    
    [field: SerializeField] public Attack[] Attacks{get; private set;} 
    [field: SerializeField] public ShiftAttack[] ShiftAttacks{get; private set;} 
    public bool isAttacking = false; 
    public Transform MainCameraTransform{get; private set;} 

    private BaseStats mainCharacterBaseStats;
    ThirdPersonWeaponConfig currentWeaponConfig;
    ThirdPersonWeapon currentWeapon;

    private bool isTwoHandsWeapon = false;
    private bool canUseSpecialAttack = false;
    private bool isVelociraptorCallingAllies = false;
    private bool isActionMusicSound = false;
    private bool isAmbientMusicSound = false;
    private bool isAction2MusicSound = false;

    private void Awake() 
    {
        currentWeaponConfig = DefaultWeapon;
        currentWeapon = SetUpDefaultWeapon();
    }

    private ThirdPersonWeapon SetUpDefaultWeapon()
    {
        return AttachWeapon(DefaultWeapon);
    }

    private ThirdPersonWeapon AttachWeapon(ThirdPersonWeaponConfig weapon)
    {
        Animator animator = GetComponent<Animator>();
        return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
    }

    public void EquipWeapon(ThirdPersonWeaponConfig weapon)
    {
        currentWeaponConfig = weapon;
        currentWeapon = AttachWeapon(weapon);
    }
    private void Start(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mainCharacterBaseStats = GetComponent<BaseStats>(); 
        MainCameraTransform = Camera.main.transform;
        EventsToPlay.AmbientMusic?.Invoke();
        isAmbientMusicSound = true;

        SwitchState(new WarriorPlayerFreeLookState(this));
    }

    public void StopParticlesEffects()
    {
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particles) {
            Destroy(ps);
        }
        Light[] lights = GetComponentsInChildren<Light>();
        foreach (Light ps in lights) {
            Destroy(ps);
        }
    }

    public void SetCanUseSpecialAttack()
    {
        canUseSpecialAttack = true;
    }

    public void SetVelociraptorCallingAllies(bool newValue)
    {
        isVelociraptorCallingAllies = newValue;
    }

    public bool GetVelociraptorCallingAllies()
    {
        return isVelociraptorCallingAllies;
    }
    
    public void StartAmbientMusic()
    {   
        if(isAmbientMusicSound){return;}
        isAmbientMusicSound = true;
        EventsToPlay.AmbientMusic?.Invoke();
    }
    public void StopAmbientMusic()
    {   
        if(!isAmbientMusicSound){return;}
        isAmbientMusicSound = false;
        EventsToPlay.StopAmbientMusic?.Invoke();
    }

    public void StartActionMusic()
    {   
        if(isActionMusicSound || isAction2MusicSound){return;}
        isActionMusicSound = true;
        EventsToPlay.ActionMusic?.Invoke();
    }

    public void StopActionMusic()
    {   
        if(!isActionMusicSound){return;}
        isActionMusicSound = false;
        EventsToPlay.StopActionMusic?.Invoke();
    }

    public void StartActionMusic2()
    {   
        if(isAction2MusicSound || isActionMusicSound){return;}
        isAction2MusicSound = true;
        EventsToPlay.ActionMusic2?.Invoke();
    }

    public void StopActionMusic2()
    {   
        if(!isAction2MusicSound){return;}
        isAction2MusicSound = false;
        EventsToPlay.StopActionMusic2?.Invoke();
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
        PlayGetHitEffect();
        if(MustProduceGetHitAnimation()){
            SwitchState(new WarriorPlayerImpactState(this));
        }
    }

    private void HandleDie()
    {
        PlayGetHitEffect();
        SwitchState(new WarriorPlayerDeadState(this));
    }

    private bool MustProduceGetHitAnimation()
    {
        int num = UnityEngine.Random.Range(0,20);
        if(num <= 6 ){
            return false;
        }     
        return true;
    }

    public float GetDamageStat(){
        return mainCharacterBaseStats.GetStat(Stat.Damage);
    }

    public bool CanStaminaPermitAttack(int attackIndex)
    {
        Attack attack = Attacks[attackIndex];
        return Stamina.CanStaminaPermitAction(attack.staminaTaked);
    }

    public bool CanStaminaPermitShiftAttack(int attackIndex)
    {
        ShiftAttack shiftAttack = ShiftAttacks[attackIndex];
        return Stamina.CanStaminaPermitAction(shiftAttack.staminaTaked);
    }

    public bool CanStaminaPermitDodge()
    {
        return Stamina.CanStaminaPermitAction(DodgeStaminaTaked);
    }

    public bool IsRighMouseClickButtonMainteinPressed()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
        {
            return true;
        }
        return false;
    }

    public bool IsShiftButtonMainteinPressed()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            return true;
        }
        return false;
    }

    public bool IsSpaceButtonPressedAndHasStamina()
    {

        if(!canUseSpecialAttack){return false;}

        if (Input.GetKey(KeyCode.Space))
        {
            float staminaConsumed = Health.GetActualMaxStaminaPoints() * 0.75f;

            return Stamina.CanStaminaPermitAction(staminaConsumed);
        }
        return false;
    }


    public void PlayGetHitEffect()
    {
        EffectsToPlay.PlayGetHitEffect();
    }

    public void PlayBlockHitEffect()
    {
        EffectsToPlay.PlayBlockHitEffect();
    }

    public bool getIsTwoHandsWeapon(){
        return this.isTwoHandsWeapon;
    }
    
    public void SetIsTwoHandsWeapon(bool newValue){
        this.isTwoHandsWeapon = newValue;
    }

    //Unity animator event
    private void WarriorRunningPlayMovementAudio1()
    {
        EventsToPlay.onMove?.Invoke();
    }

    private void WarriorRunningPlayMovementAudio2()
    {
        EventsToPlay.onMove2?.Invoke();
    }
    private void WarriorIdleStateStopSounds()
    {
        EventsToPlay.StopOnMove?.Invoke();
        EventsToPlay.StopOnMove2?.Invoke();
    }
    
    //Unity Call
    public void EnableWeapon()
    {
        GetWeaponDamage().gameObject.SetActive(true);  
    }
    //Unity Call
    public void DisableWeapon()
    {
        GetWeaponDamage().gameObject.SetActive(false);
    }

    public WeaponDamage GetWeaponDamage()
    {
        return WeaponsLogicToPlay.GetWeaponDamage(currentWeaponConfig.WeaponName);
    }
}
