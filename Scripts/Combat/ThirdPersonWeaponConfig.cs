using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Atributes;
using UnityEngine;
using RPG.Combat;

[CreateAssetMenu(fileName= "ThirdPersonWeapon", menuName ="Weapons/Make New Weapon", order =0)]
    public class ThirdPersonWeaponConfig: ScriptableObject
    {
        [SerializeField] AnimatorOverrideController weaponAnimatorOverrideController = null;
        [SerializeField] ThirdPersonWeapon ThirdPersonWeaponPrefab = null;     
        [SerializeField] ThirdPersonProjectile projectile = null; 
        [SerializeField] bool isRightHanded = true;
        [SerializeField] public bool isTwoHanded = false;
        public string WeaponName;
        const string weaponName = "CurrentWeapon";


        public ThirdPersonWeapon Spawn(Transform rightHand,Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand,leftHand);

            ThirdPersonWeapon weapon = null;
            if(ThirdPersonWeaponPrefab != null)
            {
                Transform handTransform = getHandTransform(rightHand,leftHand);
                weapon = Instantiate(ThirdPersonWeaponPrefab,handTransform);
                WeaponName = weapon.gameObject.name;
                weapon.gameObject.name = weaponName;
            }
            
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if(weaponAnimatorOverrideController != null)
            {
                animator.runtimeAnimatorController = weaponAnimatorOverrideController;
            }else if(overrideController != null)
            {//Para arreglar que pueda volver a la animación de jugador después de atacar y que no se quede en bucle atacando                                   
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
            return weapon;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }

            if(oldWeapon == null){return;}

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform getHandTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform = leftHand;

                if(isRightHanded)
                {
                    handTransform = rightHand;
                }

            return handTransform;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            ThirdPersonProjectile projectileInstance = Instantiate(projectile, getHandTransform(rightHand,leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

    }   


