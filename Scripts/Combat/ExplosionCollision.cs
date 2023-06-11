using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCollision : MonoBehaviour
{

    [SerializeField] AudioSource explosionSound;
    [SerializeField] float explosionDamage = 300f;
    [SerializeField] public bool isFromPlayer = false;
    [SerializeField] public Collider colliderToActivate;
    public float activationDelay = 1f;

    private void Start()
    {
        if(colliderToActivate != null){
            StartCoroutine(ActivateColliderAfterDelay());
        }
        
    }

    private IEnumerator ActivateColliderAfterDelay()
    {
        yield return new WaitForSeconds(activationDelay);
        colliderToActivate.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if(other.GetComponent<Health>() == null) { return;}  

        GameObject player = GameObject.FindWithTag("Player");
        if(isFromPlayer)
        {
            
            Health EnemyHealth = other.GetComponent<Health>();
            if(EnemyHealth.CheckIsDead()){return;}
            if(EnemyHealth == player.GetComponent<Health>()){return;}

            if(explosionSound != null){
                explosionSound.Play();
            }
            EnemyHealth.TakeDamage(player,explosionDamage, true, true);
        }else
        {
            Health playerHealth = player.GetComponent<Health>();
            if(playerHealth.CheckIsDead()){return;}
            if(playerHealth != other.GetComponent<Health>()){return;}
            if(explosionSound != null){
                explosionSound.Play();
            }
            
            playerHealth.TakeDamage(gameObject ,explosionDamage, true, true);
        }   
        
    }
}
