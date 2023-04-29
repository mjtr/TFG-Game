using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class ThirdPersonWeapon : MonoBehaviour
    {
       
        [SerializeField] UnityEvent onHit;
       
        public void OnHit()
        {
            onHit?.Invoke();
        }
        
    }

}
