using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;


public class WarriorPlayerSwordMagicAttack : MonoBehaviour {

	[SerializeField] private GameObject CastingEffect = null;
    [SerializeField] private GameObject PlaceToPlayCastingEffect = null; 
	[SerializeField] private GameObject LaunchEffect = null;
    [SerializeField] private GameObject PlaceToPlayLaunchEffect = null; 
	[SerializeField] private AudioSource MagicCastingAudioSource = null;
    [SerializeField] private AudioSource MagicLaunchAudioSource = null; 
	private GameObject MagicSpiritInstantiate;

	public void PlaySwordCastingMagicAudio(){
		MagicCastingAudioSource.Play();  
	}

	public void PlaySwordLaunchMagicAudio(){
		MagicLaunchAudioSource.Play();  
	}
	public void SwordCastingMagic(){
		if(PlaceToPlayCastingEffect != null && CastingEffect != null)
        {
			Transform copyEnemyTransform = PlaceToPlayCastingEffect.transform;
            MagicSpiritInstantiate = Instantiate(CastingEffect, copyEnemyTransform);
        }
	}
	public void SwordMainMagicAttack(){
		if(MagicSpiritInstantiate != null)
		{
			if(MagicSpiritInstantiate.GetComponent<ThirdPersonProjectile>() != null)
			{
				MagicSpiritInstantiate.GetComponent<ThirdPersonProjectile>().setLaunchSpeed(0.75f);
				return;
			}
			
			Destroy(MagicSpiritInstantiate);
		}

		if(PlaceToPlayLaunchEffect != null && LaunchEffect != null)
        {
			Transform copyEnemyTransform = PlaceToPlayLaunchEffect.transform;
            MagicSpiritInstantiate = Instantiate(LaunchEffect, copyEnemyTransform);
			Destroy(MagicSpiritInstantiate, 3f);
        }
		
		
	}
	
}
