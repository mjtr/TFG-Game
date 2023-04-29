using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Atributes;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class ThirdPersonWeaponPickup : MonoBehaviour
    {
        
        [SerializeField] float healthToRestore = 0f;
        [SerializeField] float respawnTime = 10;
        [SerializeField] float interactDistance = 5f;

        [SerializeField] Text interactionText;
        [SerializeField] ThirdPersonWeaponConfig weapon = null;

        private bool isInObjectRange = false;
        private GameObject playerGameObject = null;

        void OnTriggerEnter(Collider other)
        {
            
           if(other.gameObject.tag == "Player")
            {
                isInObjectRange = true;
                if(interactionText!= null)
                {
                    interactionText.gameObject.SetActive(true);
                }
                playerGameObject = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            
            if (other.CompareTag("Player"))
            {
                isInObjectRange = false;
                if(interactionText!= null)
                {
                    interactionText.gameObject.SetActive(false);
                }                
                playerGameObject = null;
            }
        }

        private void Update() {
            if (isInObjectRange && Input.GetKeyDown(KeyCode.T))
            {
                Pickup();
            }
        }


        private void Pickup()
        {
            WarriorPlayerStateMachine warriorPlayerStateMachine = playerGameObject.GetComponent<WarriorPlayerStateMachine>();
            if(healthToRestore > 0)
            {
                warriorPlayerStateMachine.SwitchState(new WarriorPlayerPickupState(warriorPlayerStateMachine,warriorPlayerStateMachine.getIsTwoHandsWeapon()));
                playerGameObject.GetComponent<Health>().Heal(healthToRestore);
                Destroy(this.gameObject);
                return;
            }
             
            if(weapon != null)
            {
                warriorPlayerStateMachine.SwitchState(new WarriorPlayerPickupState(warriorPlayerStateMachine,weapon.isTwoHanded));
                warriorPlayerStateMachine.EquipWeapon(weapon);
            }

            Destroy(this.gameObject);
        }

        //Para que reaparezca el objeto después de un tiempo
        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }


    }
}

