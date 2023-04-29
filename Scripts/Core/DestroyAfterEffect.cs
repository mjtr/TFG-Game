using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventoryExample.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] float  maxLifeTime = 10f;

        private void Start() {
            StartCoroutine(WaitBeforeDestroyEffect());
        }

         private IEnumerator WaitBeforeDestroyEffect()
        {
            yield return new WaitForSeconds(maxLifeTime);
            Destroy(gameObject);
        }
       
    }
}