using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using RPG.Stats;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class WeaponsLogicToPlay : MonoBehaviour
{
    [SerializeField] public WeaponDamage SwordWeaponLogic;
    [SerializeField] public WeaponDamage AxeWeaponLogic;
    [SerializeField] public WeaponDamage GreatSwordWeaponLogic;

    
    public WeaponDamage GetWeaponDamage(string weaponName)
    {
        if(weaponName.Contains("Axe"))
        {
            Debug.Log("Devolvemos el logic del hacha");
            return AxeWeaponLogic;
        }

        if(weaponName.Contains("GREAT"))
        {
            Debug.Log("Devolvemos el logic del greatSword");
            return GreatSwordWeaponLogic;
        }
            return SwordWeaponLogic;
    }

}
