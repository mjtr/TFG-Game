using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class SoulEaterDragonWeaponHandler : MonoBehaviour
{
    [SerializeField] public GameObject WeaponLogicHead;
    [SerializeField] public GameObject WeaponLogicTail;

//Unity Call
    public void EnableWeapon()
    {
        WeaponLogicHead.SetActive(true);
        WeaponLogicTail.SetActive(true);
    }
//Unity Call
    public void DisableWeapon()
    {
        WeaponLogicHead.SetActive(false);
        WeaponLogicTail.SetActive(false);
    }
    
}
