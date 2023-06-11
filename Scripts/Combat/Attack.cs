using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Attack
{
   
[field: SerializeField] public string AnimationName{get; private set;}

[field: SerializeField] public float TransitionDuration{get; private set;}

[field: SerializeField] public int ComboStateIndex{get; private set;} = -1;

[field: SerializeField] public float ComboAttackTime{get; private set;} 

[field: SerializeField] public float ForceTime{get; private set;}

[field: SerializeField] public float Force{get; private set;}
 
[field: SerializeField] public float ExtraDamage{get; private set;}   

[field: SerializeField] public float Knockback{get; private set;}

[field: SerializeField] public float staminaTaked{get; private set;}

[field: SerializeField] public bool isLastComboElement{get; private set;}

public void SetComboStateIndex(int newIndex){
    ComboStateIndex = newIndex;
}

}
