using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Stats;
using UnityEngine;

[Serializable]
public class Stamina : MonoBehaviour
{
    [SerializeField] BaseStats baseStats;
    [SerializeField] Health health;

    [SerializeField] float staminaRecover;
    LazyValue<float> staminaPoints;

    private void Awake() {
        staminaPoints = new LazyValue<float>(GetInitialStamina);
    }

    private float GetInitialStamina()
    {
        return GetComponent<BaseStats>().GetStat(Stat.Stamina);
    }

    void Start()
    {
        staminaPoints.ForceInit();
    }

    public void RecoverStamina()
    {
        staminaPoints.value = Mathf.Min(staminaPoints.value + staminaRecover, GetMaxStaminaPoints());
    }

    public void RegenerateStamina(float regeneratePoints)
    {
        staminaPoints.value = Mathf.Min(staminaPoints.value + regeneratePoints, GetMaxStaminaPoints());
    }

    public void TakeStamina(float staminaTaked)
    {
        staminaPoints.value = Mathf.Max(staminaPoints.value - staminaTaked, 0);
    }

    public float GetStaminaPoints()
    {
        return staminaPoints.value;
    }

    public float GetMaxStaminaPoints()
    {
        return baseStats.GetStat(Stat.Stamina);
    }


    public float GetStaminaRecover()
    {
        return staminaRecover;
    }
    public void SetStaminaRecover(float newValue)
    {
        staminaRecover = newValue;
    }

    public bool CanStaminaPermitAction(float staminaToTake)
    {
        if(staminaPoints.value >= staminaToTake){return true; }
        return false;
    }

    public float GetFraction()
    {
        return staminaPoints.value / baseStats.GetStat(Stat.Stamina);
    }

    public void RestoreStamina(float newStaminaPoints, float newStaminaRecover)
    {
        staminaPoints.value = newStaminaPoints;
        staminaRecover = newStaminaRecover;
    }
}
