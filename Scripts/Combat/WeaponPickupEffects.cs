using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickupEffects : MonoBehaviour
{

    [field: SerializeField] public GameObject WeaponPickupEffect;
    private GameObject effectInstantiate;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (effectInstantiate == null)
            {
                effectInstantiate = Instantiate(WeaponPickupEffect, transform.position, Quaternion.identity);
                effectInstantiate.transform.parent = transform;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (effectInstantiate != null)
            {
                Destroy(effectInstantiate);
                effectInstantiate = null;
            }
        }
    }
}
