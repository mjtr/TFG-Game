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

    [SerializeField] public UnityEvent AmbientMusic;
    [SerializeField] public UnityEvent StopAmbientMusic;

    [SerializeField] public UnityEvent ActionMusic;
    [SerializeField] public UnityEvent StopActionMusic;

    [SerializeField] public UnityEvent ActionMusic2;
    [SerializeField] public UnityEvent StopActionMusic2;

    [System.Serializable]
    public class TakeDamageEvent : UnityEvent<float>{ }
}
