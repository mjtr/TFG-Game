using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EventsToPlay : MonoBehaviour
{
    [SerializeField] public TakeDamageEvent onTakeDamage; 
    [SerializeField] public UnityEvent onDie;
    [SerializeField] public UnityEvent onAttack;
    [SerializeField] public UnityEvent onTailAttack;
    [SerializeField] public UnityEvent onMove;
    [SerializeField] public UnityEvent onMove2;

    [SerializeField] public UnityEvent StopOnMove;
    [SerializeField] public UnityEvent StopOnMove2;

    [SerializeField] public UnityEvent WarriorOnAttack;
    [SerializeField] public UnityEvent ShieldBlock;

    [SerializeField] public UnityEvent WarriorOnPosionEffect;
    [SerializeField] public UnityEvent Breath;

    [SerializeField] public UnityEvent Shout;
    [SerializeField] public UnityEvent GetHit;

    [SerializeField] public UnityEvent OnAttack2;
    [SerializeField] public UnityEvent OnAttack3;

     [SerializeField] public UnityEvent takeFireAttack;

    [System.Serializable]
    public class TakeDamageEvent : UnityEvent<float>{ }
}
