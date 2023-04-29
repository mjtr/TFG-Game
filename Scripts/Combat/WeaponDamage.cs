using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] private Collider myCollider;
    private float damage;
    private float knockback;

    private List<Collider> alreadyCollidedWith = new List<Collider>();

    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }
    
    private void OnTriggerEnter(Collider other){
        if(myCollider == null)
        {
            myCollider = gameObject.GetComponent<CharacterController>();
        }

        if(other == myCollider){return;}

        if(alreadyCollidedWith.Contains(other)){return;}

        if(other.tag == myCollider.tag){return;} 

        alreadyCollidedWith.Add(other);

        if(other.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(myCollider.gameObject, damage, false);
        }

        if(other.TryGetComponent<ForceReceived>( out ForceReceived forceReceived))
        {
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized; 
            
            forceReceived.AddForce(direction * knockback);
        }

    }

    public void SetAttack(float damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }
}
