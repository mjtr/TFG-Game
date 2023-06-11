using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class Health : MonoBehaviour
{
    [SerializeField] BaseStats baseStats;
    [SerializeField] EventsToPlay eventsToPlay;
  
    public event Action OnTakeDamageForInvokeImpactState;
    public event Action OnDieForInvokeDeadState;

    LazyValue<float> healthPoints;
    private bool IsInvulnerable = false;
    private bool isDead = false;
    
    private void Awake() {
        healthPoints = new LazyValue<float>(GetInitialHealth);
    }

    private float GetInitialHealth()
    {
        return GetComponent<BaseStats>().GetStat(Stat.Health);
    }

    void Start()
    {
        healthPoints.ForceInit();
    }

    private void OnEnable() {
        baseStats.OnLevelUp += RegenerateHealthAndStamina;
    }

    private void OnDisable() {
        baseStats.OnLevelUp -= RegenerateHealthAndStamina;
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.IsInvulnerable = isInvulnerable;
    }

    public void TakeDamage(GameObject instigator, float damage, bool isFromProyectile, bool produceHitsEffects)
    {
        if(healthPoints.value <= 0){ return; }

        if(IsInvulnerable)
        {
            if(instigator.tag == "Player" )
            {
                EventsToPlay eventsToPlay = gameObject.GetComponent<EventsToPlay>();
                EffectsToPlay effectsToPlay = gameObject.GetComponent<EffectsToPlay>();
                
                eventsToPlay.ShieldBlock?.Invoke();
                effectsToPlay.PlayBlockHitEffect();
            }
            else
            {
                FindAndPlayPlayerBlockEvents();
                
            }
           return;
        }

        healthPoints.value = Mathf.Max(healthPoints.value - damage,0);

        if(healthPoints.value == 0f)
        {
            Die();
            AwardExperience(instigator);
        }else
        {
            if(!IsInvulnerable && instigator.tag != "Player" && isFromProyectile )
            {
               OnTakeDamageForInvokeImpactState?.Invoke(); //Para cambiar al impactState
               eventsToPlay?.onTakeDamage.Invoke(damage);//Para el cambio de vida en las barras
            }

            if(!IsInvulnerable && !isFromProyectile){
                if(produceHitsEffects){
                    OnTakeDamageForInvokeImpactState?.Invoke(); //Para cambiar al impactState
                    eventsToPlay?.onTakeDamage.Invoke(damage);//Para el cambio de vida en las barras
                }
            }
        }
    }

    public bool GetIsInvulnerable()
    {
        return this.IsInvulnerable;
    }

    public void FindAndPlayPlayerBlockEvents()
    {
        WarriorPlayerStateMachine warriorStateMachine =  GameObject.FindWithTag("Player").GetComponent<WarriorPlayerStateMachine>();
        warriorStateMachine.EventsToPlay.ShieldBlock?.Invoke();
        warriorStateMachine.PlayBlockHitEffect();
    }

   
    public void Heal(float healToRestore)
    {
        healthPoints.value = Mathf.Min(healthPoints.value + healToRestore,GetMaxHealthPoints());
    }

    public float GetHealthPoints()
    {
        return healthPoints.value;
    }

    public float GetMaxHealthPoints()
    {
        return baseStats.GetStat(Stat.Health);
    }
    public float GetPercentage()
    {
        return 100 * GetFraction();
    }

    public float GetFraction()
    {
        return healthPoints.value / baseStats.GetStat(Stat.Health);
    }

    private void Die()
    {
        if(CheckIsDead()){return;}
        eventsToPlay.onDie?.Invoke();
        isDead = true;
        OnDieForInvokeDeadState?.Invoke();
    }

    public void AwardExperience(GameObject instigator)
    {
        Experience experience = instigator.GetComponent<Experience>();
        if(experience == null){return;}

        experience.GainExperience(baseStats.GetStat(Stat.ExperienceReward));
    }

    private void RegenerateHealthAndStamina()
    {
        healthPoints.value = baseStats.GetStat(Stat.Health);
        GameObject.FindWithTag("Player").GetComponent<Stamina>().RegenerateStamina(baseStats.GetStat(Stat.Stamina));
    }

    public bool CheckIsDead()
    {
        return isDead;
    }

    public float GetActualMaxStaminaPoints()
    {
        return baseStats.GetStat(Stat.Stamina);
    }

    public void RestoreHealthPoints(float healthPoints)
    {
        this.healthPoints.value = healthPoints; 
    }

    public void RestoreLevel(int newLevel)
    {
        this.baseStats.RestoreLevel(newLevel); 
    }

}
