using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySecondWeaponHandler : MonoBehaviour
{

    [SerializeField] private GameObject weaponLogic;
    
//Unity Call
    public void EnableSecondWeapon()
    {
        weaponLogic.SetActive(true);
    }
//Unity Call
    public void DisableSecondWeapon()
    {
        weaponLogic.SetActive(false);
    }
    
}
