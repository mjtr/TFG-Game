using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThirdWeaponHandler : MonoBehaviour
{

    [SerializeField] private GameObject weaponLogic;
    
//Unity Call
    public void EnableThirdWeapon()
    {
        weaponLogic.SetActive(true);
    }
//Unity Call
    public void DisableThirdWeapon()
    {
        weaponLogic.SetActive(false);
    }
    
}
