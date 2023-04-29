using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonWeaponHandler : MonoBehaviour
{
    [SerializeField] public GameObject WeaponLogicHead;
    [SerializeField] public GameObject WeaponLogicBody;
    [SerializeField] public GameObject WeaponLogicTail;
    [SerializeField] public GameObject WeaponLogicFinishTail;

//Unity Call
    public void EnableWeapon()
    {
        WeaponLogicHead?.SetActive(true);
        WeaponLogicBody?.SetActive(true);
        WeaponLogicTail?.SetActive(true);
        WeaponLogicFinishTail?.SetActive(true);
    }
//Unity Call
    public void DisableWeapon()
    {
        WeaponLogicHead?.SetActive(false);
        WeaponLogicBody?.SetActive(false);
        WeaponLogicTail?.SetActive(false);
        WeaponLogicFinishTail?.SetActive(false);
    }

    public void EnableHeadWeapon()
    {
        WeaponLogicHead?.SetActive(true);
    }

    public void EnableAllWeapon()
    {
        WeaponLogicHead?.SetActive(true);
        WeaponLogicBody?.SetActive(true);
        WeaponLogicTail?.SetActive(true);
        WeaponLogicFinishTail?.SetActive(true);
    }
    
     public void DisableHeadWeapon()
    {
        WeaponLogicHead?.SetActive(false);
    }

    public void DisableAllWeapon()
    {
        WeaponLogicHead?.SetActive(false);
        WeaponLogicBody?.SetActive(false);
        WeaponLogicTail?.SetActive(false);
        WeaponLogicFinishTail?.SetActive(false);
    }
    
}
