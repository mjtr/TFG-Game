using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponHandler : MonoBehaviour
{

    [SerializeField] private GameObject weaponLogic;
    
//Unity Call
    public void EnableWeapon()
    {
        weaponLogic.SetActive(true);
    }
//Unity Call
    public void DisableWeapon()
    {
        weaponLogic.SetActive(false);
    }
    
}
