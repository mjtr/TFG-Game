using RPG.Combat;
using RPG.Stats;
using UnityEngine;


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

    private bool canUseNewComboAttack = false;

    private int actionMusicSoundIndex;
    private int ambientSoundIndex;
    private bool isBossMusicSound;

    private AudioController audioController;

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

    public void RestoreWeapon(ThirdPersonWeaponConfig weapon)
    {
        DefaultWeapon = weapon;
        currentWeaponConfig = DefaultWeapon;
        currentWeapon = SetUpDefaultWeapon();
    }

    public void EquipWeapon(ThirdPersonWeaponConfig weapon)
    {
        currentWeaponConfig = weapon;
        currentWeapon = AttachWeapon(weapon);
    }
    private void Start(){
        if(ES3.KeyExists("FirstTimeToSaveInScene")){
            Transform transformSaved = ES3.Load<Transform>("PlayerTransform");
            gameObject.transform.SetPositionAndRotation(transformSaved.position, transformSaved.rotation);
            gameObject.transform.localScale = transformSaved.localScale;
            gameObject.transform.SetLocalPositionAndRotation(transformSaved.localPosition, transformSaved.localRotation);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        actionMusicSoundIndex = -1;
        ambientSoundIndex = -1;
        isBossMusicSound = false;
        audioController = GetComponent<AudioController>();
   
        mainCharacterBaseStats = GetComponent<BaseStats>(); 
        MainCameraTransform = Camera.main.transform;
        StartAmbientMusic();

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

    public void RestoreSpecialAttack(bool newValue)
    {
        canUseSpecialAttack = newValue;
    }

    public bool GetCanUseSpecialAttack()
    {
        return canUseSpecialAttack;
    }

    public bool GetCanUseNewComboAttack()
    {
        return canUseNewComboAttack;
    }


    public void AddNewComboAttack(){
        canUseNewComboAttack = true;
        for (int i = 0; i < Attacks.Length; i++)
        {
            Attack attack = Attacks[i];
            if(attack.isLastComboElement)
            {
                attack.SetComboStateIndex(3);
            }
        }

        for (int i = 0; i < Attacks.Length; i++)
        {
            ShiftAttack shiftAttack = ShiftAttacks[i];
            if(shiftAttack.isLastComboElement)
            {
                shiftAttack.SetComboStateIndex(3);
            }
        }

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
        if(isBossMusicSound){return;}
        if(ambientSoundIndex >= 0 || actionMusicSoundIndex >= 0){return;}
        
        AudioSource[] ambientsAudioSource = audioController.AmbientMusics;
        ambientSoundIndex = UnityEngine.Random.Range(0,ambientsAudioSource.Length);

        AudioSource ambientAudioSelected = ambientsAudioSource[ambientSoundIndex];
        ambientAudioSelected.Play();
    }

    public void StopAmbientMusic()
    {   
        if(ambientSoundIndex < 0 || actionMusicSoundIndex > -1){return;}
        
        AudioSource ambientAudioSourceToStop = audioController.AmbientMusics[ambientSoundIndex]; 
        ambientSoundIndex = -1;
        ambientAudioSourceToStop.Stop();
    }

    public void StartActionMusic()
    {   
        if(isBossMusicSound){return;}
        if(actionMusicSoundIndex > -1){return;}

        AudioSource[] actionsAudioSource = audioController.ActionMusics;
        actionMusicSoundIndex = UnityEngine.Random.Range(0,actionsAudioSource.Length);

        AudioSource actionAudioSelected = actionsAudioSource[actionMusicSoundIndex];
        actionAudioSelected.Play();
    }

    public void StopActionMusic()
    {   
        if(actionMusicSoundIndex < 0){return;}
        bool existOtherEnemiesAtacking = false;
        foreach(var target in Targeter.GetPlayerTargets())
        {
            if(target != null){
                AudioController audioController = target.GetComponent<AudioController>();
                if(audioController != null)
                {
                    existOtherEnemiesAtacking = audioController.GetIsMonsterAttacking();
                    if(existOtherEnemiesAtacking){return;}
                }
            }
           
        }

        AudioSource actionAudioSourceToStop = audioController.ActionMusics[actionMusicSoundIndex]; 
        actionMusicSoundIndex = -1;
        actionAudioSourceToStop.Stop();
    }

    public void StartEpicMusic()
    {   
        if(actionMusicSoundIndex > -1 ){
            AudioSource actionAudioSourceToStop = audioController.ActionMusics[actionMusicSoundIndex]; 
            actionMusicSoundIndex = -1;
            actionAudioSourceToStop.Stop();
        }

        if(ambientSoundIndex > -1 ){
            AudioSource ambientAudioSourceToStop = audioController.AmbientMusics[ambientSoundIndex]; 
            ambientSoundIndex = -1;
            ambientAudioSourceToStop.Stop();
        }
        isBossMusicSound = true;
        audioController.BossMusic.Play();
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

    public Transform GetRightHandTransform(){
        return this.rightHandTransform;
    }

    public Transform GetLeftHandTransform(){
        return this.leftHandTransform;
    }
    
    public void RestoreHandsTransform(Transform rightHand, Transform leftHand){
        this.rightHandTransform.transform.SetPositionAndRotation(rightHand.position, rightHand.rotation);
        this.rightHandTransform.transform.localScale = rightHand.localScale;

        this.leftHandTransform.transform.SetPositionAndRotation(leftHand.position, leftHand.rotation);
        this.leftHandTransform.transform.localScale = leftHand.localScale;
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
