using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMagics : MonoBehaviour
{
    [SerializeField] private float posionDamage; 

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            WarriorPlayerStateMachine warriorStateMachine = GameObject.FindWithTag("Player").GetComponent<WarriorPlayerStateMachine>();
            warriorStateMachine.EventsToPlay.WarriorOnPosionEffect?.Invoke();
            warriorStateMachine.Health.TakeDamage(this.gameObject,posionDamage,false);
        }
    }
}
