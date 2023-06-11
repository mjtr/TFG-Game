using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using RPG.Stats;
using UnityEngine.Events;

public class FarmAnimalStateMachine : StateMachine
{
    [field: SerializeField] public Animator Animator{get; private set;} 
 
    private BaseStats FarmAnimalBaseStats;
    private void Start()
    {
        FarmAnimalBaseStats = GetComponent<BaseStats>(); 

        SwitchState(new FarmAnimalEatingState(this));
        return;
    }
    
}
