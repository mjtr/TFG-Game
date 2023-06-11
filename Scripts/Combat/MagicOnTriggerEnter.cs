using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicOnTriggerEnter : MonoBehaviour
{
    [SerializeField] public float damage = 0; 
    [SerializeField] public Collider colliderToActivate;
    public float activationDelay = 1f;
    public float desactivationnDelay = 0f;

    private bool firstTime = true;

    private void Start()
    {
        if(colliderToActivate != null){
            StartCoroutine(ActivateColliderAfterDelay());
            if(desactivationnDelay != 0f)
            {
                StartCoroutine(DesactivateColliderAfterDelay());
            }
        }
    }

    private IEnumerator ActivateColliderAfterDelay()
    {
        yield return new WaitForSeconds(activationDelay);
        colliderToActivate.enabled = true;
        colliderToActivate.isTrigger = true;
    }

     private IEnumerator DesactivateColliderAfterDelay()
    {
        yield return new WaitForSeconds(desactivationnDelay);
        colliderToActivate.enabled = false;
        colliderToActivate.isTrigger = false;
    }

    void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            WarriorPlayerStateMachine warriorStateMachine = GameObject.FindWithTag("Player").GetComponent<WarriorPlayerStateMachine>();
            if(firstTime)
            {
                warriorStateMachine.EventsToPlay.takeFireAttack?.Invoke();
                firstTime = false;
            }
            warriorStateMachine.Health.TakeDamage(this.gameObject,damage,false, false);
        }
    }
}
