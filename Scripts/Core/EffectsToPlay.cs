using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EffectsToPlay : MonoBehaviour
{
    [SerializeField] private GameObject GetHitEffect = null;
    [SerializeField] private GameObject PlaceToPlayGetHitEffect = null; 
    [SerializeField] private GameObject WarriorBlockHitEffect = null;
    [SerializeField] private GameObject WarriorPlaceToPlayBlockHitEffect = null; 


    public void PlayGetHitEffect()
    {
        if(GetHitEffect != null && PlaceToPlayGetHitEffect != null)
        {
            Transform copyEnemyTransform = PlaceToPlayGetHitEffect.transform;
            GameObject effect = Instantiate(GetHitEffect, copyEnemyTransform);
            Destroy(effect, 0.7f);
        } 
    }

    public void PlayBlockHitEffect()
    {
        if(WarriorBlockHitEffect != null && WarriorPlaceToPlayBlockHitEffect != null)
        {
            Transform copyEnemyTransform = WarriorPlaceToPlayBlockHitEffect.transform;
            GameObject effect = Instantiate(WarriorBlockHitEffect, copyEnemyTransform);
            Destroy(effect, 0.5f);
        } 
    }

}
