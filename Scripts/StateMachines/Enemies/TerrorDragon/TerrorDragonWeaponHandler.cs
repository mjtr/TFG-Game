using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class TerrorDragonWeaponHandler : MonoBehaviour
{
    [SerializeField] public GameObject WeaponLogicHead;
    [SerializeField] public GameObject WeaponLogicRightWing;
    [SerializeField] public GameObject WeaponLogicLeftWing;

//Unity Call
    public void EnableHeadWeapon()
    {
        WeaponLogicHead.SetActive(true);
    }

    public void EnableWingsWeapon()
    {
        WeaponLogicRightWing.SetActive(true);
        WeaponLogicLeftWing.SetActive(true);
    }
//Unity Call
    public void DisableHeadWeapon()
    {
        WeaponLogicHead.SetActive(false);
    }

    public void DisableWingWeapon()
    {
        WeaponLogicRightWing.SetActive(false);
        WeaponLogicLeftWing.SetActive(false);
    }
    
}
