using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Target : MonoBehaviour
{

   [SerializeField] Health targetHealth;

   public event Action<Target> onDestroyed;

   private void onDestroy(){
      onDestroyed?.Invoke(this);
   }

   public Health GetTargetHealth(){
      return targetHealth;
   }

}
